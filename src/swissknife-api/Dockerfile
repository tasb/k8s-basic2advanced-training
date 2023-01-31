FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
LABEL turma=6
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR "/src/swissknife-api"
COPY ["swissknife-api.csproj", "."]

RUN dotnet restore "swissknife-api.csproj"
COPY . .
RUN dotnet build "swissknife-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "swissknife-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "swissknife-api.dll"]
