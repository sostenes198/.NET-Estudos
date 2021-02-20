Configurações debug:

host start --port 7071 --pause-on-error

Para utilizar no rider devser instalar o azure storage emulator manualmente:

https://docs.microsoft.com/pt-br/azure/storage/common/storage-use-emulator


Verificar o status se esta rodando:

"%programfiles(x86)%\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" status
"%programfiles(x86)%\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" init
"%programfiles(x86)%\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" start