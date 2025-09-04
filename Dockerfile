ARG BASE_TAG=8.0

# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:${BASE_TAG} AS build
WORKDIR /src
COPY ./API/API.sln ./
COPY ./API/API.API/API.API.csproj ./API.API/
COPY ./API/API.Domain/API.Domain.csproj ./API.Domain/
COPY ./API/API.Application/API.Application.csproj ./API.Application/
COPY ./API/API.Infrastructure/API.Infrastructure.csproj ./API.Infrastructure/
RUN dotnet restore ./API.API/API.API.csproj
COPY ./API/ .
RUN dotnet publish ./API.API/API.API.csproj -c Release -o /app /p:UseAppHost=false

# ===== runtime =====
# Si es un Worker puro, usa "runtime"; si sirve HTTP/ASP.NET, usa "aspnet"
FROM mcr.microsoft.com/dotnet/aspnet:${BASE_TAG} AS runtime
WORKDIR /app
COPY --from=build /app ./
COPY ./API/API.API/Data/app.db ./Data/
RUN adduser --disabled-password appuser && chown -R appuser:appuser /app
USER appuser
ENTRYPOINT ["dotnet", "API.API.dll"]

# ===== tests =====
FROM mcr.microsoft.com/dotnet/sdk:${BASE_TAG} AS tests
WORKDIR /app
COPY ./API/API.sln ./
COPY ./API/API.API/API.API.csproj ./API.API/
COPY ./API/API.Domain/API.Domain.csproj ./API.Domain/
COPY ./API/API.Application/API.Application.csproj ./API.Application/
COPY ./API/API.Infrastructure/API.Infrastructure.csproj ./API.Infrastructure/
COPY ./API/API.Tests/API.Tests.csproj ./API.Tests/
RUN dotnet restore --force --no-cache
COPY ./API/ .
RUN dotnet build API.sln --no-restore
CMD ["dotnet", "test", "API.sln", "--no-build", "--verbosity", "normal"]
