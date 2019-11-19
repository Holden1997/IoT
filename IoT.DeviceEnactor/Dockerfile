FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build

WORKDIR /src
COPY *.sln .
COPY ["IoT.DeviceEnactor/IoT.DeviceEnactor.csproj", "IoT.DeviceEnactor/"]
COPY ["IoT.Common/IoT.Common.csproj", "IoT.Common/"]
RUN dotnet restore "IoT.DeviceEnactor/IoT.DeviceEnactor.csproj"
COPY . .
WORKDIR "/src/IoT.DeviceEnactor"
RUN dotnet build "IoT.DeviceEnactor.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IoT.DeviceEnactor.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IoT.DeviceEnactor.dll"]