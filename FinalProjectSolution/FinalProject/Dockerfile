FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY FinalProject/FinalProject.csproj FinalProject/
COPY Shared/Shared.csproj Shared/
RUN dotnet restore FinalProject/FinalProject.csproj

COPY . .
WORKDIR /src/FinalProject
RUN dotnet publish FinalProject.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FinalProject.dll"]
