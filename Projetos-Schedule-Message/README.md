## Comando para builder a imagem
    docker build -t schedule/message:1.0 --progress=plain --no-cache .

## Comando para executar container docker
    docker run -d -p 3000:3000 -e "Mongo__Hangfire__ConnectionString=mongodb://scheduledmessagehangfire-mongo1-1:27017/test?directConnection=true" --network scheduledmessagehangfire_mongo-cluster schedule/message:1.0 
