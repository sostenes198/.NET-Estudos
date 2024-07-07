# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy default files
COPY NuGet.Config .

# copy csproj
#COPY *.sln .
COPY ./src/**/*.csproj ./src/
COPY ./tests/**/*.csproj ./tests/
RUN  for file in src/*.csproj; do filename=$(basename -s .csproj $file); filepath="src/$filename"; mkdir $filepath; mv $file $filepath; done
RUN  for file in tests/*.csproj; do filename=$(basename -s .csproj $file); filepath="tests/$filename"; mkdir $filepath; mv $file $filepath; done

# restore as distinct layers
RUN dotnet clean
RUN dotnet restore

# copy everything else and build app
COPY src/. ./src/
COPY src/. ./tests
WORKDIR /source/src/Scheduled.Message.Api
RUN dotnet publish -c Release -o /app --no-cache

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# expose
EXPOSE 3000

#variables
ARG AllowedHosts
ARG JFROG_TOKEN
ARG Logging__LogLevel__Default
ARG Logging__LogLevel__Microsoft_AspNetCore
ARG Logging__LogLevel__Hangfire
ARG Mongo__Hangfire__ConnectionString
ARG Mongo__Hangfire__HangfireParametersCollection
ARG Mongo__Hangfire__CreateIndex__Enabled
ARG Gateways__VollScheduler__BaseUrl
ARG Hangfire__TtlHangfireDocumentInDays

ENV AllowedHosts="*"
ENV Logging__LogLevel__Default=""
ENV Logging__LogLevel__Microsoft_AspNetCore="Warning"
ENV Logging__LogLevel__Hangfire="Warning"
ENV Mongo__Hangfire__ConnectionString="mongodb://localhost:27017/test?directConnection=true"
ENV Mongo__Hangfire__HangfireParametersCollection="hangfire.parameters"
ENV Mongo__Hangfire__CreateIndex__Enabled="True"
ENV Mongo__Hangfire__CreateIndex__TTL__Enabled="True"
ENV Gateways__VollScheduler__BaseUrl="https://48825ab4-fbf1-4c44-95c1-ff8386b10aca.mock.pstmn.io"
ENV Hangfire__TtlHangfireDocumentInDays="60"

ENV ASPNETCORE_URLS="http://+:3000"

# execute app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Scheduled.Message.Api.dll"]