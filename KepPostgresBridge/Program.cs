using System;
using Npgsql;

namespace KepPostgresBridge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var conexao = "Host=localhost;Username=gui;Password=senha123;Database=usina_virtual";

            using var conn = new NpgsqlConnection(conexao);

            try
            {
                conn.Open();
                Console.WriteLine("✅ Conectado ao banco de dados PostgreSQL com sucesso!");

                var comando = new NpgsqlCommand(
                    "INSERT INTO sensores_teste (nome, valor) VALUES (@nome, @valor)", conn);
                comando.Parameters.AddWithValue("nome", "temperatura_dorna01");
                comando.Parameters.AddWithValue("valor", 33.5);

                var linhasAfetadas = comando.ExecuteNonQuery();
                Console.WriteLine($"👍 {linhasAfetadas} linha(s) inserida(s) com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro: " + ex.Message);
            }
        }
    }
}
