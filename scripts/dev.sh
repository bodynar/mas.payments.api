#!/bin/bash

# Dev mode: all services, DB without persistent volume (data is lost on container removal)
# Usage: ./scripts/dev.sh [--build]

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

cd "$PROJECT_DIR"

BUILD_FLAG=""
if [ "$1" = "--build" ]; then
    BUILD_FLAG="--build"
fi

docker compose --env-file .env -f docker-compose.yml -f docker-compose.dev.yml up $BUILD_FLAG
