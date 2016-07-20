FROM microsoft/dotnet:1.0.0-core

WORKDIR /app

ENV HOST_URI http://0.0.0.0:5000/
ENV API_BASE_URI http://load-balancer/

EXPOSE 5000

ENTRYPOINT ["dotnet", "website.dll"]

COPY artifacts /app
