# 📦 01 — Embeddings

> Aprenda o conceito fundamental de IA Generativa: transformar texto em vetores e armazená-los para busca semântica.

---

## 🧠 Conceitos

### O que é um Embedding?

Um embedding é a representação numérica de um texto — um array de números (vetor) que captura o **significado semântico** do conteúdo.

Textos com significados parecidos geram vetores próximos no espaço vetorial, permitindo **busca por similaridade** em vez de busca por palavras-chave exatas.

```
"cachorro"  → [0.12, 0.87, 0.03, ...]
"cão"       → [0.13, 0.85, 0.04, ...]   ← próximo!
"automóvel" → [0.91, 0.02, 0.67, ...]   ← distante
```

### O que o exemplo faz?

```
1. Recebe uma lista de textos
         ↓
2. Envia cada texto para o Ollama (mxbai-embed-large)
         ↓
3. Recebe um vetor de 1024 dimensões por texto
         ↓
4. Salva os vetores no PostgreSQL via EF Core
         ↓
5. Realiza uma busca semântica por similaridade
```

---

## 🛠️ Tecnologias

| Tecnologia | Papel |
|---|---|
| .NET 10 | Plataforma |
| EF Core | ORM + Migrations |
| PostgreSQL + pgvector | Armazenamento de vetores |
| Ollama | Servidor de modelos local |
| mxbai-embed-large | Modelo de embedding |

---

## ▶️ Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)

---

## 🚀 Rodando o projeto

**1. Suba os serviços**
```bash
docker-compose up -d
```

Isso vai iniciar o PostgreSQL com pgvector e o Ollama, e já faz o download do modelo `mxbai-embed-large` automaticamente.

**2. Aplique as migrations**
```bash
dotnet ef database update
```

**3. Rode o projeto**
```bash
dotnet run
```

---

## 🐳 Serviços Docker

| Serviço | Porta | Descrição |
|---|---|---|
| PostgreSQL | `5432` | Banco com extensão pgvector |
| Ollama | `11434` | Servidor de modelos local |

---

## 📂 Estrutura

```
01-Embeddings/
├── docker-compose.yml  # Sobe PostgreSQL e Ollama
├── Program.cs          # Fluxo principal
├── appsettings.json    # String de conexão e configurações
└── IAGenerativa.Embeddings.csproj
```
