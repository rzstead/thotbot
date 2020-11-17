FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Thot.Listener/Thot.Listener.csproj", "Thot.Listener/"]
RUN dotnet restore "Thot.Listener/Thot.Listener.csproj"
COPY . .
WORKDIR "/src/Thot.Listener"
RUN dotnet build "Thot.Listener.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Thot.Listener.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Thot.Listener.dll"]
