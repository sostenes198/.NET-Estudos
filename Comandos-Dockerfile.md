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