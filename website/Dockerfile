FROM microsoft/aspnetcore:1.0.1

ENV HOST_URI http://0.0.0.0:5000/
ENV API_BASE_URI http://load-balancer/
EXPOSE 5000

WORKDIR /app
COPY ./artifacts /app

ENTRYPOINT ["dotnet", "website.dll"]