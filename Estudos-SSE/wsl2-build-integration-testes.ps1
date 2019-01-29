#! /bin/bash
wsl docker-compose --file docker-compose-test.yml down
wsl docker-compose --file docker-compose-test.yml up -d