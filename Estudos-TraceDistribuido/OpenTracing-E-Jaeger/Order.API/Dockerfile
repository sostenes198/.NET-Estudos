FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app

RUN dotnet --version

COPY ./Order.API/*.csproj ./Order.API/
COPY ./Common/*.csproj ./Common/

RUN dotnet restore ./Order.API/*.csproj
COPY . ./
RUN dotnet publish ./Order.API/*.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
COPY --from=build /out /out
WORKDIR /out
EXPOSE 80

ENTRYPOINT ["dotnet", "Order.API.dll"]