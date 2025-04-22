# 🏭 Usina GUI - Simulador de Planta de Etanol com OPC UA

Este repositório contém o projeto completo da simulação de uma usina de etanol automatizada, criada com o objetivo de aprendizado e testes de integração com o sistema BioCal.

---

## 📂 Estrutura de Pastas

```
usina-gui/
├── LeitorOpcUa/                  # Projeto C# que lê dados OPC UA e grava no PostgreSQL
│   ├── Program.cs                # Código principal com loop de leitura
│   ├── OpcUaClient.Config.xml    # Configurações de certificado OPC UA
│   └── LeitorOpcUa.csproj        # Arquivo do projeto C#
│
├── tags/                         # Arquivos de configuração com endereços das TAGs OPC
│   └── tags.json
│
├── tagsCSV_exportKeepServer/    # CSVs exportados do KEPServerEX com as TAGs simuladas
│   ├── Caldeira.csv
│   ├── Dorna01.csv
│   └── ...
│
├── docs/                         # Documentação do projeto
│   └── estrutura.md
│
└── README.md                     # Este arquivo
```

---

## ⚙️ Tecnologias Utilizadas

- **C#** (.NET 8)
- **OPC UA** (KepServerEX)
- **PostgreSQL** (via Docker)
- **Npgsql** (Driver .NET para PostgreSQL)
- **Git & GitHub** (versionamento)
- (Em breve) **Grafana** para visualização dos dados

---

## 🧠 Objetivo

Simular uma planta industrial de etanol com sensores e atuadores OPC UA, registrar os dados em um banco de dados relacional e permitir a futura visualização em dashboards analíticos.

---

## 👨‍💻 Como executar

1. Inicie o KEPServerEX com as TAGs simuladas
2. Certifique-se de que o PostgreSQL está rodando com a porta correta (5433)
3. Execute o `LeitorOpcUa.exe` (loop contínuo)
4. Dados serão gravados automaticamente na tabela `leituras_opcua`

---

## 📬 Contato

Desenvolvido por [Guilherme Xavier](https://www.linkedin.com/in/xavierguilherme)
