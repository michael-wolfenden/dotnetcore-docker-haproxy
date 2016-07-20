FROM microsoft/dotnet:1.0.0-core

WORKDIR /app

ENV HOST_URI http://0.0.0.0:5000/

EXPOSE 5000

ENTRYPOINT ["dotnet", "api.dll"]

COPY artifacts /app
