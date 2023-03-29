#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Service2/Service2.csproj", "src/Service2/"]
COPY ["src/Distributed.Tracing/Distributed.Tracing.csproj", "src/Distributed.Tracing/"]
RUN dotnet restore "src/Service2/Service2.csproj"
COPY . .
WORKDIR "/src/src/Service2"
RUN dotnet build "Service2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service2.dll"]