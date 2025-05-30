#!/bin/bash
set -euo pipefail

log() {
  echo ""
  echo "$1"
  echo ""
}

error_exit() {
  echo ""
  echo "[X] Error on line $1. Exiting."
  echo ""
  exit 1
}

trap 'error_exit $LINENO' ERR

log "Building Docker image..."
docker build -t telemetry-gen:latest .

log "Deploying to Kubernetes..."
kubectl apply -f telemetry-gen.yaml

log "[✓] Deployment complete!"
