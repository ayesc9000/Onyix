FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Onyix/Onyix.csproj", "Onyix/"]
RUN dotnet restore "Onyix/Onyix.csproj"
COPY . .
WORKDIR "/src/Onyix"
RUN dotnet build "Onyix.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Onyix.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Onyix.dll"]
