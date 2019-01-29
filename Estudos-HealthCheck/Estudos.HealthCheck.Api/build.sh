#! /bin/bash
docker compose --file docker-compose-integration.yml down
docker compose --file docker-compose-integration.yml up -d