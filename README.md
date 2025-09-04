# Good practices

El presente repositorio contiene una API desarrollada `limpiamente` con buenas prácticas de programación. Cada commit contiene un paso del desarrollo de la API con los comandos correspondientes para mejor seguimiento.

> **Nota**: El repositorio está pensado para ser ejecutado y desarrollado en contenedores, por lo que los entornos, como Node, no serán necesarios a menos que el desarrollador prefiera instalar las dependencias manualmente en su sistema.

- [1. Requisitos](#1-requisitos)
- [2. Get started](#2-get-started)
- [2.1. Ejecución en contenedores](#21-ejecucion-en-contenedores)
- [3. Configuración](#3-configuración)
- [3.1. Base de datos](#31-base-de-datos)
- [3.2. Migraciones](#32-migraciones)
- [4. Arquitectura](#4-arquitectura)
- [5. ToDos](#5-todos)

## 1. Requisitos

- [Docker](https://docs.docker.com/get-started/get-docker/)
- [dotnet-sdk-8.0](https://learn.microsoft.com/en-gb/dotnet/core/install/)

## 2. Get started

### 2.1. Ejecución en contenedores

Para ejecutar y probar la API como runtime de producción, será necesario ejecutar el siguiente comando:

```terminal
docker-compose --profile api up -d
```

En caso de querer ejecutar y/o revisar las pruebas unitarias, se podrá probar con el siguiente comando:

```terminal
docker-compose run --rm api
```

## 3. Configuración

### 3.1. Base de datos

La API está diseñada para trabajar con SQLite, si el motor de base de datos es distinto, se deberán instalar los paquetes manualmente en la API y verificar que no _se rompa_, así como efectuar las migraciones manualmente. Good lock!

### 3.2. Migraciones

Para realizar migraciones en contenedores:

```PowerShell
docker run --rm `
-v "$(pwd):/app" `
-w /app `
mcr.microsoft.com/dotnet/sdk:8.0 `
bash -c 'dotnet tool install --global dotnet-ef --version 8.0.17 && export PATH="$PATH:/root/.dotnet/tools" && dotnet ef migrations add [MigrationName] && dotnet ef database update'
```

## 4. Arquitectura

```
/API
├─ API.API
│ ├─ Common/
│ └─ Endpoints/
├─ API.Application
│ ├─ Common/
│ └─ Services/
│ │ ├─ Contracts/
│ │ ├─ Create/
│ │ ├─ Delete/
│ │ ├─ DTOs/
│ │ └─ GetById/
├─ API.Domain
│ ├─ Abstractions/
│ ├─ Common/
│ └─ Services/
├─ API.Infrastructure
│ ├─ Persistence/
│ │ ├─ Configurations/
│ │ └─ Interceptors/
│ └─ Repositories/
└─ API.Tests
  └─ Services/
```

## 5. ToDos

- Migracianos de las entidades.
- Agregar Identity para autentificación en la API.
- Uso de Serilog.
- Buenas prácticas en el desarrollo de una App con Angular y consumo de API.