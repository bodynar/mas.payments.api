#!/bin/bash

# Backend mode: only DB + backend, no frontend
# Usage: ./scripts/backend.sh [--build]

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

cd "$PROJECT_DIR"

BUILD_FLAG=""
if [ "$1" = "--build" ]; then
    BUILD_FLAG="--build"
fi

docker compose --env-file .env up $BUILD_FLAG db mas-payments
