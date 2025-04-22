
# 🏭 Usina Virtual - Leitor OPC UA

Este projeto simula uma usina de etanol e realiza a leitura contínua de variáveis de processo via protocolo OPC UA. As leituras são armazenadas automaticamente em um banco de dados PostgreSQL para futura análise com ferramentas como o Grafana.

## 🔧 Tecnologias Utilizadas
- C# (.NET 8)
- OPC UA .NET Standard Library (OPC Foundation)
- KEPServerEX (com driver Simulator OPC UA)
- PostgreSQL (via Docker)
- Git + GitHub

## 📁 Estrutura do Projeto

```
usina-gui/
├── LeitorOpcUa/               # Projeto em C# que lê as TAGs via OPC UA e grava no PostgreSQL
│   ├── Program.cs             # Código principal do leitor
│   └── OpcUaClient.Config.xml# (em breve) Configurações de segurança OPC UA
│
├── tags/                      # Arquivos JSON com endereços das TAGs OPC UA reais
│   └── tags.json
│
├── tagsCSV_exportKeepServer/ # Arquivos .csv usados para gerar as TAGs no KEPServerEX
│   └── *.csv
│
├── docs/
│   └── estrutura.md           # Documento explicando a estrutura do projeto
│
├── .gitignore
└── README.md
```

## ⚙️ Executando o Leitor

Certifique-se de que:
1. O **KEPServerEX** está rodando com as TAGs configuradas e acessíveis via OPC UA.
2. O **PostgreSQL** (Docker) está em execução na porta `5433`, com:
   - Usuário: `gui`
   - Senha: `1234`
   - Banco: `usina_virtual`
3. O arquivo `tags.json` contém os caminhos OPC UA no formato completo (ex: `ns=2;s=SimuladorRandom.Dorna01.Temperatura_PV`)

### Rodar via Visual Studio:
- Abra a solução `LeitorOpcUa.sln`
- Execute como **Administrador**
- Verifique no console se as variáveis estão sendo gravadas com sucesso

---

**Em breve:**
- Visualização com Grafana
- Empacotamento em `.exe`
- Deploy com Docker Compose

---

Desenvolvido por [Guilherme Xavier](https://www.linkedin.com/in/xavierguilherme)
