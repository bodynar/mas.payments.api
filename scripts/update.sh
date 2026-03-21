#!/bin/bash

# Update existing application: backup DB, rebuild backend, apply migrations, restart
# Usage: ./scripts/update.sh [--skip-backup]

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

cd "$PROJECT_DIR"

source .env 2>/dev/null || true

DB_CONTAINER="${COMPOSE_PROJECT_NAME:-$(basename "$PROJECT_DIR" | tr '[:upper:]' '[:lower:]' | tr -d '.')}-db-1"
DB_NAME="${POSTGRES_DB:-prt}"
DB_USER="${POSTGRES_USER:-devw}"
BACKUP_DIR="$PROJECT_DIR/db"
BACKUP_FILE="$BACKUP_DIR/${DB_NAME}_backup_$(date +%Y%m%d_%H%M%S).dump"

echo "=== Updating MAS.Payments ==="

# 1. Check that DB container is running
echo ""
echo "[1/5] Checking DB container..."
if ! docker inspect "$DB_CONTAINER" > /dev/null 2>&1; then
    echo "Error: DB container '$DB_CONTAINER' not found."
    echo "Start the application first with ./scripts/backend.sh"
    exit 1
fi

if [ "$(docker inspect -f '{{.State.Running}}' "$DB_CONTAINER")" != "true" ]; then
    echo "Error: DB container '$DB_CONTAINER' is not running."
    exit 1
fi

echo "DB container '$DB_CONTAINER' is running."

# 2. Backup database
if [ "$1" != "--skip-backup" ]; then
    echo ""
    echo "[2/5] Creating database backup..."
    mkdir -p "$BACKUP_DIR"
    docker exec "$DB_CONTAINER" pg_dump -U "$DB_USER" -d "$DB_NAME" -F c -f /tmp/db_backup.dump
    docker cp "$DB_CONTAINER:/tmp/db_backup.dump" "$BACKUP_FILE"
    docker exec "$DB_CONTAINER" rm /tmp/db_backup.dump
    echo "Backup saved to: $BACKUP_FILE"
else
    echo ""
    echo "[2/5] Skipping backup (--skip-backup flag)."
fi

# 3. Rebuild backend image
echo ""
echo "[3/5] Rebuilding backend image..."
docker compose --env-file .env build mas-payments

# 4. Apply EF migrations
echo ""
echo "[4/5] Applying database migrations..."
docker compose --env-file .env run --rm --no-deps mas-payments \
    sh -c "dotnet MAS.Payments.dll --ef-migrate" 2>/dev/null || {
    # Fallback: apply migrations via dotnet ef from host if available
    if command -v dotnet &> /dev/null; then
        CONNECTION_STRING="${CONNECTION_STRING:-Host=localhost;Port=${POSTGRES_PORT:-5401};Pooling=true;Database=$DB_NAME;Username=$DB_USER;Password=${POSTGRES_PASSWORD:-123};Timeout=5;}"
        ConnectionStrings__DefaultConnection="$CONNECTION_STRING" dotnet ef database update --project "$PROJECT_DIR"
    else
        echo "Warning: Could not apply migrations automatically."
        echo "Run 'dotnet ef database update' manually."
    fi
}

# 5. Restart backend
echo ""
echo "[5/5] Restarting backend..."
docker compose --env-file .env up -d --no-deps mas-payments

echo ""
echo "=== Update complete ==="
docker compose --env-file .env ps db mas-payments
