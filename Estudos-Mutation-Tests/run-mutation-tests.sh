# !/bin/bash

DOTNET_STRYKER_COMMAND="dotnet-stryker"
CURRENT_VERSION="3.2.0"
NOT_INSTALED_DOTNETSTRYKER="command not found"

# Condição utilizada pela pipelina para sempre instalar a ferramente do dotnet-stryker
if [[ $1 == 'true' ]]; then
echo "Instalando dotnet-stryker pela condição de pipeline"
dotnet tool install -g dotnet-stryker --version $CURRENT_VERSION --ignore-failed-sources --add-source 'https://api.nuget.org/v3/index.json'  
fi

OUTPUT=$($DOTNET_STRYKER_COMMAND -s NONE)

echo "Current Version: ${CURRENT_VERSION}"

if [[ $OUTPUT == "" ]];then
    echo "Instalando dotnet-stryker"
    dotnet tool install -g dotnet-stryker --version $CURRENT_VERSION --ignore-failed-sources --add-source 'https://api.nuget.org/v3/index.json'
elif [[ !($OUTPUT == *"$CURRENT_VERSION"*) ]]; then
    echo "Atualizando dotnet-stryker para versão atual utilizada"
    dotnet tool uninstall -g dotnet-stryker
    dotnet tool install -g dotnet-stryker --version $CURRENT_VERSION --ignore-failed-sources --add-source 'https://api.nuget.org/v3/index.json'
fi

FolderOutputStryker='.\StrykerOutput'

if [ -d "$FolderOutputStryker" ]; then rm -rf $FolderOutputStryker; fi

BASH_EXIT_RESULT=0
DOTNET_STRYKER_PROJECTS=(
"{{INSIRA_O_PROJETO_AQUI}}"
"{{INSIRA_O_PROJETO_AQUI}}"
)
  
for project in "${DOTNET_STRYKER_PROJECTS[@]}"
do
  RESULT=$($DOTNET_STRYKER_COMMAND -f ./stryker-config.json -p $project)
  echo "$RESULT" 
  
  if [[ $RESULT == *"Final mutation score is below threshold break. Crashing"* ]]; then
    echo "Resultado com falha bash será finalizado com erro"
    BASH_EXIT_RESULT=1
  fi
done

exit $BASH_EXIT_RESULT