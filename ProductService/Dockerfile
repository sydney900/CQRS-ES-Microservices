FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src/CQRS.Core
COPY CQRS.Core/CQRS.Core.csproj .
COPY CQRS.Core/. .
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Release

WORKDIR /src/CQRS.Domain
COPY CQRS.Domain/CQRS.Domain.csproj .
COPY CQRS.Domain/. .
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Release

WORKDIR /src/SecurityShare
COPY SecurityShare/SecurityShare.csproj .
COPY SecurityShare/. .
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Release

WORKDIR /src/ProductService
COPY ProductService/ProductService.csproj ProductService/
COPY ProductService/. .
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ProductService.dll"]
