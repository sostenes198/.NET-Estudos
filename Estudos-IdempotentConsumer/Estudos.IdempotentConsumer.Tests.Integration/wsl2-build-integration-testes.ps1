#! /bin/bash
wsl docker-compose --file docker-compose-integration.test.yml down
wsl docker-compose --file docker-compose-integration.test.yml up -d