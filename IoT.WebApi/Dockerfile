FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5090

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build

WORKDIR /src
COPY *.sln .
COPY ["IoT.WebApi/IoT.WebApi.csproj", "IoT.WebApi/"]
COPY ["IoT.Domain.Interfaces/IoT.Domain.Interfaces.csproj", "IoT.Domain.Interfaces/"]
COPY ["IoT.Domain/IoT.Domain.csproj", "IoT.Domain/"]
COPY ["IoT.Infrastructure/IoT.Infrastructure.csproj", "IoT.Infrastructure/"]
COPY ["IoT.Infrastructure.Interfaces/IoT.Infrastructure.Interfaces.csproj", "IoT.Infrastructure.Interfaces/"]
COPY ["IoT.IoC/IoT.IoC.csproj", "IoT.IoC/"]
COPY ["IoT.Common/IoT.Common.csproj", "IoT.Common/"]

RUN dotnet restore "IoT.WebApi/IoT.WebApi.csproj"
COPY . .
WORKDIR "/src/IoT.WebApi"
RUN dotnet build "IoT.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IoT.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IoT.WebApi.dll"]