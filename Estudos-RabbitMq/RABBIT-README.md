Container rabbit: 

docker run -d — hostname rabbit — name rabbit -p 15672:15672 -p 5672:5672 -p 25676:25676 rabbitmq:3-management