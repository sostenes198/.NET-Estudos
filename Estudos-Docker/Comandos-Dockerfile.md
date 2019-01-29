Exemplo Dockerfile básico

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY **/*.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Poc.TestesIntegradosJep.dll"]

----------------------------------------------------------------------------------------------------------------------------

REMOVER Imagens None
docker image rm $(docker image ls -f 'dangling=true' -q)

s
COMANDO DOCKER

Builda a imagem definindo um nome
docker build -t aspnetapp .


docker image build -t functionestudossosoapp -f devops/Dockerfile  --no-cache . 
(Exemplo de como buildar uma com o arquivo Dockerfile dentro de uma subpasta)
(OBS: Deve-se builder da raiz do projeto que é aonde o dockerengine ira visualizar o sistema de arquivos)


Exemplo para criar um container expondo a porta 80 do container na porta 8080 local
docker run -d -p 8080:80 --name myapp aspnetapp

Remove imagem com Id específico
docker rmi $id_imagem 

Remove container específico
docker rm $id_container

Cria um arquivo referente a imagem docker e salva uma imagem docker no diretório definido no parâmetro -o 
docker save -o <path for generated tar file> <image name>

Carrega uma imagem salva para dentro do docker
docker load -i <path to image tar file>

Entra no bash do container informado
docker exec -it <id container> /bin/sh (Entra no container por linha de comando)

Entrar no bash do container com usuário root
docker exec -it --user=root <id_container> /bin/sh

Quando a imagem linux é alphine usar comandos
apk update e apk add

Network Docker:
    Criando rede docker:
        docker network create -d bridge my-net
    Associando Container existente a uma network:
        docker network connet {{NOME_NETWORK}} {{NOME_OU_ID_CONTAINER}}
    Criando container associando uma network:
        docker run --network=my-net -itd --name=container3 busybox
    Inspecionando Network:
        docket network inspect {{NOME_NETWORK}}





------------------------------------------------------------------ Exemplo docker-compose (SQLSERVER) rodando script ------------------------------------------------------------------

version: "3.4"

services:
  SqlServer:
    container_name: SqlServer
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - MSSQL_SA_PASSWORD=Pass@word
      - ACCEPT_EULA=Y
    ports:
      - "1433:1433"
    volumes:
      - "DIRETORIO_DOS_SCRIPTS:/scripts/"
    command:
      - /bin/bash
      - -c
      - |
        /opt/mssql/bin/sqlservr &
        sleep 30
        /opt/mssql-tools/bin/sqlcmd -U sa -P Pass@word -l 30 -e -i /scripts/NOME_DO_SCRIPT.sql
        sleep infinity

  Redis:
    container_name: Redis
    image: redis
    command: redis-server --requirepass Pass@word
    ports:
      - "7000:6379"
    depends_on:
      - FacialBiometricsSqlServer
  MongoDb:
      container_name: MongoDb
      image: mongo
      restart: always
      environment:
        MONGO_INITDB_ROOT_USERNAME: admin
        MONGO_INITDB_ROOT_PASSWORD: Pass@word
        MONGO_INITDB_DATABASE: admin
      ports:
        - 27017:27017
      command:
        - "--auth"
      volumes:
        - "/scripts/NOME_DO_SCRIPT.js:/docker-entrypoint-initdb.d/mongo-init.js:ro"

-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------