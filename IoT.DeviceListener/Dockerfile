FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY *.sln .
COPY ["IoT.DeviceListener/IoT.DeviceListener.csproj", "IoT.DeviceListener/"]
COPY ["IoT.Common/IoT.Common.csproj", "IoT.Common/"]
RUN dotnet restore "IoT.DeviceListener/IoT.DeviceListener.csproj"
COPY . .
WORKDIR "/src/IoT.DeviceListener"
RUN dotnet build "IoT.DeviceListener.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IoT.DeviceListener.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IoT.DeviceListener.dll"]