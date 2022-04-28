#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Level.Dinamico/Level.Dinamico.csproj", "Level.Dinamico/"]
RUN dotnet restore "Level.Dinamico/Level.Dinamico.csproj"
COPY . .
WORKDIR "/src/Level.Dinamico"
RUN dotnet build "Level.Dinamico.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Level.Dinamico.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Level.Dinamico.dll"]