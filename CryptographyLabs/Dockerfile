FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CryptographyLabs/CryptographyLabs.csproj", "CryptographyLabs/"]
RUN dotnet restore "CryptographyLabs/CryptographyLabs.csproj"
COPY . .
WORKDIR "/src/CryptographyLabs"
RUN dotnet build "CryptographyLabs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptographyLabs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptographyLabs.dll"]
