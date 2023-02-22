Link do tutorial: https://sipkeschoorstra.medium.com/building-workflow-driven-net-applications-with-elsa-2-part-1-44e08a9ba94b

Container docker para subir aplicação:

docker run -p 3000:80 -p 2525:25 rnwood/smtp4dev:linux-amd64-3.1.0-ci0856

docker run -d -t -i -e ELSA__SERVER__BASEADDRESS='http://localhost:5130' -p 14000:80 elsaworkflows/elsa-dashboard:latest

curl --location 'http://localhost:5130/v1/workflows/58a38494bd6d456ea1743e67b30cb2fc/execute' \
--header 'Content-Type: application/json' \
--data '{
"correlationId": "c2165b1a1ba946e0a03ab4d6d65d7df3"
}'