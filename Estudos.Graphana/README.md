# Instalando grafana

docker run -d --name=grafana -p 3000:3000 grafana/grafana

## Configurando Grafana

Acessar container do grafana

acessar pasta etc/grafana (cd /etc/grafanaa)
criar copia do arquivo grafa.ini (sudo cp grafana.ini custom.ini)
editar arquivo grafana.ini

## Instalar plugins no grafana

Acessar container do grafana
acessar pasta bin (cd bin)
Utlizar grafana-cli

Exemplo: grafana-cli plugins install briangann-gauge-panel

# Instalando graphite

docker run --name graphite --restart=always -p 81:81 -p 8125:8125/udp hopsoft/graphite-stats

## Configurando graphite

acessar container do grafite

acessar pasta opt/graphite/conf (cd /opt/graphite/conf)
editar arquivo storage-schemas.conf

Criar configuração shoehub

[shoehub]
pattern = ^shoeshub\.
retentions = 20s:5h

