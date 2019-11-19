FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
	
EXPOSE 1884 

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY *.sln .
COPY ["IoT.Mqtt.Broker/IoT.Mqtt.Broker.csproj", "IoT.Mqtt.Broker/"]
RUN dotnet restore "IoT.Mqtt.Broker/IoT.Mqtt.Broker.csproj"
COPY . .
WORKDIR "/src/IoT.Mqtt.Broker"
RUN dotnet build "IoT.Mqtt.Broker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IoT.Mqtt.Broker.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IoT.Mqtt.Broker.dll"]