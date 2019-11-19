FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app

EXPOSE 5090

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY *.sln .
COPY ["IoT.Mvc/IoT.Mvc.csproj", "IoT.Mvc/"]
RUN dotnet restore "IoT.Mvc/IoT.Mvc.csproj"
COPY . .
WORKDIR "/src/IoT.Mvc"
RUN dotnet build "IoT.Mvc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IoT.Mvc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IoT.Mvc.dll"]