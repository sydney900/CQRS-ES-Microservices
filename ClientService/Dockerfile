FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY CqrsMicroservices.sln ./
COPY ClientService/ClientService.csproj ClientService/
COPY CQRS.Domain/CQRS.Domain.csproj CQRS.Domain/
COPY CQRS.Core/CQRS.Core.csproj CQRS.Core/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/ClientService
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClientService.dll"]
