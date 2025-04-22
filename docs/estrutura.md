# 📘 Documentação - Estrutura da Usina GUI

Este documento explica a estrutura do projeto `usina-gui`.

## 🧱 Componentes Simulados

### Dornas (Dorna01, Dorna02, Dorna03, Dorna04)
- Simulam a fermentação do mosto.
- TAGs: temperatura, pH, nível, volume, etanol, alarmes, etc.

### CIP01
- Sistema de limpeza Clean-In-Place.
- TAGs: temperatura, válvulas, bomba, alarme de temperatura.

### Destilaria
- Destilação do vinho para produção do etanol.
- TAGs: temperatura, pressão, nível, refluxo, caldeira.

### Energia
- Monitoramento elétrico da planta.
- TAGs: potência, corrente, tensão, fator de potência, consumo.

### Utilidades
- Infraestrutura geral da usina.
- TAGs: vapor, ar comprimido, água, gerador, reservatórios.

### Caldeira, Laboratório, Silo01, RecebimentoCana
- Sistemas auxiliares que enriquecem a simulação.

## 💾 Armazenamento
- Banco de dados PostgreSQL (rodando via Docker).
- Aplicação C# insere dados de TAGs no banco.

## 🌍 Visão futura
- Conectar com Grafana.
- Automatizar leitura do OPC da planta.
- Simular integração com BioCal.
