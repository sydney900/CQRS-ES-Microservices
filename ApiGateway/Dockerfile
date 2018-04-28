FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY CqrsMicroservices.sln ./
COPY ApiGateway0/ApiGateway0.csproj ApiGateway0/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/ApiGateway0
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiGateway0.dll"]
