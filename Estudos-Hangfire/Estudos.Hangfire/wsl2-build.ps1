#! /bin/bash
wsl docker-compose --file docker-compose-build.yml down
wsl docker-compose --file docker-compose-build.yml up -d