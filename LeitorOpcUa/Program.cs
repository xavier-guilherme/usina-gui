using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Npgsql;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🔐 Iniciando conexão segura com o servidor OPC UA...");

        // 1. Cria e configura a aplicação OPC UA
        var config = new ApplicationConfiguration()
        {
            ApplicationName = "LeitorOpcUa",
            ApplicationType = ApplicationType.Client,
            SecurityConfiguration = new SecurityConfiguration
            {
                ApplicationCertificate = new CertificateIdentifier
                {
                    StoreType = "Directory",
                    StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Applications",
                    SubjectName = "CN=LeitorOpcUa, O=MinhaEmpresa, C=BR"
                },
                TrustedPeerCertificates = new CertificateTrustList
                {
                    StoreType = "Directory",
                    StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA CertificateAuthorities"
                },
                AutoAcceptUntrustedCertificates = true,
                AddAppCertToTrustedStore = true
            },
            TransportConfigurations = new TransportConfigurationCollection(),
            TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
            ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
            TraceConfiguration = new TraceConfiguration()
        };

        await config.Validate(ApplicationType.Client);

        var app = new ApplicationInstance
        {
            ApplicationName = "LeitorOpcUa",
            ApplicationType = ApplicationType.Client,
            ApplicationConfiguration = config
        };

        // 2. Verifica ou gera certificado
        if (!await app.CheckApplicationInstanceCertificate(false, 0))
        {
            throw new Exception("❌ Certificado de aplicação inválido!");
        }

        // 3. Cria a sessão OPC UA
        var endpointURL = "opc.tcp://localhost:49320/";
        var endpoint = CoreClientUtils.SelectEndpoint(endpointURL, false);
        var session = await Session.Create(config, new ConfiguredEndpoint(null, endpoint), false, "LeitorOpcUa", 60000, null, null);

        Console.WriteLine("✅ Conectado com sucesso!\n");

        // 4. Lê o JSON externo com os caminhos das tags
        Console.WriteLine("📥 Lendo arquivo de TAGs JSON...");
        var jsonPath = @"C:\usina-gui\tags\tags.json";
        var tagPaths = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(jsonPath));
        if (tagPaths == null || tagPaths.Count == 0)
        {
            Console.WriteLine("⚠️ Nenhuma TAG encontrada no JSON.");
            return;
        }

        // 5. Prepara os NodeIds para leitura
        var nodesToRead = new ReadValueIdCollection();
        foreach (var tag in tagPaths)
        {
            nodesToRead.Add(new ReadValueId
            {
                NodeId = new NodeId(tag),
                AttributeId = Attributes.Value
            });
        }

        // Loop de execução contínua
        while (true)
        {
            // 6. Faz a leitura das TAGs
            Console.WriteLine("\n📡 Iniciando leitura das variáveis...");
            session.Read(
                null,
                0,
                TimestampsToReturn.Both,
                nodesToRead,
                out DataValueCollection results,
                out DiagnosticInfoCollection diagnosticInfos
            );
            Console.WriteLine($"📊 {results.Count} variáveis encontradas.");

            // 7. Conecta ao banco PostgreSQL
            Console.WriteLine("🗄️ Gravando no banco de dados...");
            string connString = "Host=localhost;Port=5433;Username=gui;Password=1234;Database=usina_virtual";
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // 8. Grava os dados lidos no banco
            for (int i = 0; i < tagPaths.Count; i++)
            {
                var tag = tagPaths[i];
                var valor = results[i].Value?.ToString() ?? "NULL";
                var timestamp = results[i].SourceTimestamp.ToUniversalTime();

                await using var cmd = new NpgsqlCommand(
                    "INSERT INTO leituras_opcua (tag, valor, timestamp) VALUES (@tag, @valor, @timestamp)", conn);

                cmd.Parameters.AddWithValue("tag", tag);
                cmd.Parameters.AddWithValue("valor", valor);
                cmd.Parameters.AddWithValue("timestamp", timestamp);

                await cmd.ExecuteNonQueryAsync();

                Console.WriteLine($"📝 Gravado: {tag} = {valor} ({timestamp})");
            }

            Console.WriteLine("⏳ Aguardando próxima leitura...\n");

            // 9. Aguarda intervalo antes da próxima leitura (ex: 5 segundos)
            await Task.Delay(5000); // <-- Altere aqui se quiser outro intervalo (em milissegundos)
        }
    }
}