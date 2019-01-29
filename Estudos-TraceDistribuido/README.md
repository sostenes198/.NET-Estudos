Links Uteis:
[https://docs.microsoft.com/pt-br/dotnet/core/diagnostics/distributed-tracing](https://docs.microsoft.com/pt-br/dotnet/core/diagnostics/distributed-tracing)

[https://opentelemetry.io/docs/](https://opentelemetry.io/docs/)
[https://opentelemetry.io/docs/instrumentation/net/](https://opentelemetry.io/docs/instrumentation/net/)


Container Jaeger:
docker run --name jaeger -d -p 5775:5775/udp -p 6831:6831/udp -p 6832:6832/udp -p 5778:5778 -p 16686:16686 -p 14268:14268 -p 9411:9411 jaegertracing/all-in-one:latest

```yaml
version: '3.4'

services:
  jaeger: 
        image: jaegertracing/all-in-one:latest
        ports:
          - "5775:5775/udp"
          - "6831:6831/udp"
          - "6832:6832/udp"
          - "5778:5778"
          - "16686:16686"
          - "14268:14268"
          - "9411:9411"
```