FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY EmailWorker/EmailWorker.csproj EmailWorker/
COPY Shared/Shared.csproj Shared/
RUN dotnet restore EmailWorker/EmailWorker.csproj

COPY . .
WORKDIR /src/EmailWorker
RUN dotnet publish EmailWorker.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EmailWorker.dll"]
