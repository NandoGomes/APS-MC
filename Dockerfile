FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .

COPY APS.MC.Shared/*.csproj ./APS.MC.Shared/
RUN dotnet restore ./APS.MC.Shared

COPY APS.MC.Domain/*.csproj ./APS.MC.Domain/
RUN dotnet restore ./APS.MC.Domain

COPY APS.MC.Infra/*.csproj ./APS.MC.Infra/
RUN dotnet restore ./APS.MC.Infra

COPY APS.MC.API/*.csproj ./APS.MC.API/
RUN dotnet restore ./APS.MC.API

# copy everything else and build app
COPY . ./
WORKDIR /app/APS.MC.API
RUN dotnet publish -c Release -o out

RUN ls /app/APS.MC.API/out/

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/APS.MC.API/out ./
ENTRYPOINT ["dotnet", "APS.MC.API.dll"]