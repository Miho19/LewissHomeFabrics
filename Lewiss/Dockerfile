FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5085

LABEL org.opencontainers.image.source=https://github.com/miho19/LewissHomeFabrics.git

ENV ASPNETCORE_URLS=http://+:5085

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Lewiss.Pricing.Api/Lewiss.Pricing.Api.csproj", "Lewiss.Pricing.Api/"]
COPY ["Lewiss.Pricing.Api.Tests/Lewiss.Pricing.Api.Tests.csproj", "Lewiss.Pricing.Api.Tests/"]
COPY ["Lewiss.Pricing.Data/Lewiss.Pricing.Data.csproj", "Lewiss.Pricing.Data/"]
COPY ["Lewiss.Pricing.Shared/Lewiss.Pricing.Shared.csproj", "Lewiss.Pricing.Shared/"]
RUN dotnet restore "Lewiss.Pricing.Api/Lewiss.Pricing.Api.csproj"

COPY . .

WORKDIR /src/Lewiss.Pricing.Api.Tests
RUN dotnet test

# build / publish
WORKDIR "/src/Lewiss.Pricing.Api"
RUN dotnet build "Lewiss.Pricing.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Lewiss.Pricing.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lewiss.Pricing.Api.dll"]
