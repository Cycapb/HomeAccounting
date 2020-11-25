#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebUI.Core/WebUI.Core.csproj", "WebUI.Core/"]
COPY ["BussinessLogic/BussinessLogic.csproj", "BussinessLogic/"]
COPY ["DomainModels/DomainModels.csproj", "DomainModels/"]
COPY ["Loggers/Loggers.csproj", "Loggers/"]
COPY ["Providers/Providers.csproj", "Providers/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Loggers.Extensions/Loggers.Extensions.csproj", "Loggers.Extensions/"]
COPY ["DomainModels.EntityORM.Core/DomainModels.EntityORM.Core.csproj", "DomainModels.EntityORM.Core/"]
COPY ["DomainModels.EntityORM/DomainModels.EntityORM.csproj", "DomainModels.EntityORM/"]
RUN dotnet restore "WebUI.Core/WebUI.Core.csproj"
COPY . .
WORKDIR "/src/WebUI.Core"
RUN dotnet build "WebUI.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebUI.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebUI.Core.dll"]