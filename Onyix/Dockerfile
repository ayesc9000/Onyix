# Onyix - An open-source Discord bot
# Copyright (C) 2022 Liam "AyesC" Hogan
#
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program. If not, see http://www.gnu.org/licenses/.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
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
RUN apt-get install libopus0 libopus-dev libsodium23 libsodium-dev
ENTRYPOINT ["dotnet", "Onyix.dll"]
