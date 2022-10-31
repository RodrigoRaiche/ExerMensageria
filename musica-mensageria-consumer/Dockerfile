FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["musica-mensageria-consumer/musica-mensageria-consumer.csproj", "musica-mensageria-consumer/"]
RUN dotnet restore "musica-mensageria-consumer/musica-mensageria-consumer.csproj"
COPY . .
WORKDIR "/src/musica-mensageria-consumer"
RUN dotnet build "musica-mensageria-consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "musica-mensageria-consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "musica-mensageria-consumer.dll"]
