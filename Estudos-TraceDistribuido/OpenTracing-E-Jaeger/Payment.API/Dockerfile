FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app

RUN dotnet --version

COPY ./Payment.API/*.csproj ./Payment.API/
COPY ./Common/*.csproj ./Common/

RUN dotnet restore ./Payment.API/*.csproj
COPY . ./
RUN dotnet publish ./Payment.API/*.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
COPY --from=build /out /out
WORKDIR /out
EXPOSE 80

ENTRYPOINT ["dotnet", "Payment.API.dll"]