## PostgreSQL (Docker) — Workplace Tasks

Este repositório contém um `docker-compose.yml` simples para subir um PostgreSQL para o desafio **"Workplace Tasks"** descrito em `Desafio_Tecnico_Dev_FullStack_NET_Angular_PostgreSQL.pdf`.

### Requisitos

- Docker + Docker Compose (plugin `docker compose`)

### Subir o banco

Opcional: ajuste as variáveis em `env.example` (ou exporte no shell).

Subindo com os valores default:

```bash
docker compose up -d
```

Parar/remover:

```bash
docker compose down
```

### Dados default

- **Host**: `localhost`
- **Porta**: `5432`
- **Database**: `workplace_tasks`
- **User**: `workplace`
- **Password**: `workplace`

### String de conexão (exemplos)

- **Npgsql / .NET (appsettings.json)**:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=workplace_tasks;Username=workplace;Password=workplace"
  }
}
```

- **psql**:

```bash
psql "postgresql://workplace:workplace@localhost:5432/workplace_tasks"
```

