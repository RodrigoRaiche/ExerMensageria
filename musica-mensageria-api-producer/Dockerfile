FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["musica-mensageria-api-producer/musica-mensageria-api-producer.csproj", "musica-mensageria-api-producer/"]
RUN dotnet restore "musica-mensageria-api-producer/musica-mensageria-api-producer.csproj"
COPY . .
WORKDIR "/src/musica-mensageria-api-producer"
RUN dotnet build "musica-mensageria-api-producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "musica-mensageria-api-producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "musica-mensageria-api-producer.dll"]
