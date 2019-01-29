Subir servidor Seq

docker run -d --restart unless-stopped --name seq -e ACCEPT_EULA=Y -p 5341:80 datalust/seq