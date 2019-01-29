"AzureWebJobsStorage": "UseDevelopmentStorage=true",
"ConnectionString": "UseDevelopmentStorage=true",
"StorageConnectionString": "UseDevelopmentStorage=true",

Configurações debug:

host start --port 7071 --pause-on-error

Para utilizar no rider devser instalar o azure storage emulator manualmente:

https://docs.microsoft.com/pt-br/azure/storage/common/storage-use-emulator


Verificar o status se esta rodando:

"%programfiles(x86)%\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" status
"%programfiles(x86)%\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" init
"%programfiles(x86)%\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" start

### OBS: Azure storage emulator esta obsoleto

## Usando Azurite:

docker run -p 10000:10000 -p 10001:10001 --name azurite -d mcr.microsoft.com/azure-storage/azurite azurite --blobPort 10000 --blobHost 0.0.0.0 --queuePort 10001 --queueHost 0.0.0.0

**Comando completo com volume**
docker run -p 10000:10000 -p 10001:10001 -v c:/azurite:/workspace --name azurite -d mcr.microsoft.com/azure-storage/azurite azurite -l /workspace -d /workspace/debug.log --blobPort 10000 --blobHost 0.0.0.0 --queuePort 10001 --queueHost 0.0.0.0 --loose --skipApiVersionCheck