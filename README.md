# MAS.Payments API

REST API for tracking payments and meter measurements. Built with ASP.NET Core 9.0, PostgreSQL, CQRS.

## Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://docs.docker.com/get-docker/) and Docker Compose

## Setup

Copy the environment variables file and edit as needed:

```bash
cp .env.example .env
```

### Environment Variables

| Variable | Description | Default |
|---|---|---|
| `FRONTEND_PATH` | Path to the frontend directory | `../../frontend/MAS.Payments.Client` |
| `FRONTEND_PORT` | Frontend port on host | `5050` |
| `BACKEND_PORT` | Backend port inside the container | `80` |
| `POSTGRES_IMAGE` | PostgreSQL Docker image | `postgres:15` |
| `POSTGRES_DB` | Database name | `prt` |
| `POSTGRES_USER` | Database user | `devw` |
| `POSTGRES_PASSWORD` | Database password | `123` |
| `POSTGRES_PORT` | PostgreSQL port on host | `5401` |
| `CONNECTION_STRING` | Database connection string | `Host=db;Port=5432;...` |

## Launch Modes

### 1. Full Launch (production)

All services: frontend + backend + DB with persistent volume.

```bash
docker compose up --build
```

- Frontend: `http://localhost:5050`
- DB accessible on host: `localhost:5401`
- DB data persists in Docker volume `p_payments`

### 2. Dev Mode

All services, but DB **without persistent volume** — data is lost when containers are stopped. Useful for development with a clean DB.

```bash
./scripts/dev.sh          # start
./scripts/dev.sh --build  # with image rebuild
```

### 3. Backend Only

DB + backend without frontend. For API development or when running the frontend separately.

```bash
./scripts/backend.sh          # start
./scripts/backend.sh --build  # with image rebuild
```

### 4. Local Launch without Docker

Requires a running PostgreSQL instance. Connection is configured in `appsettings.json`:

```bash
dotnet run
```
