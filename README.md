# AgroOrbit — Advanced Business Development with .NET

## Visão geral

A **AgroOrbit** é uma API REST desenvolvida em **ASP.NET Core 8** para apoiar o monitoramento agrícola com dados de satélite, sensores IoT e análise de risco climático.

Ela representa a entrega da disciplina **Advanced Business Development with .NET** da Global Solution 2026/1.

## Ideia da solução

A solução permite que produtores, cooperativas ou técnicos agrícolas acompanhem fazendas e talhões, registrem sensores, leituras de campo e dados orbitais como NDVI, temperatura de superfície, cobertura de nuvens e focos de calor.

A partir desses dados, a API classifica o status do talhão, gera alertas climáticos e recomenda ações ao produtor.

## Funcionalidades principais

- Cadastro de usuários
- Cadastro de fazendas
- Cadastro de talhões
- Cadastro de sensores IoT
- Registro de leituras dos sensores
- Registro de dados satelitais
- Cálculo automático de risco agrícola
- Geração de alertas climáticos
- Geração de recomendações
- Dashboard administrativo
- Swagger/OpenAPI
- Banco PostgreSQL
- Docker

## Tecnologias utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Swagger/OpenAPI
- Docker
- C#
- REST API

## Estrutura do projeto

```text
AgroOrbit-DotNet-TOP
├── src
│   └── AgroOrbit.Api
│       ├── Controllers
│       ├── Data
│       ├── Domain
│       │   ├── Entities
│       │   └── Enums
│       ├── DTOs
│       │   ├── Requests
│       │   └── Responses
│       ├── Middlewares
│       ├── Services
│       ├── Program.cs
│       └── Dockerfile
├── docs
├── scripts
├── docker-compose.yml
└── README.md
```

## Relacionamentos do banco

- User 1:N Farm
- Farm 1:N CropArea
- CropArea 1:N Sensor
- Sensor 1:N SensorReading
- CropArea 1:N SatelliteData
- CropArea 1:N ClimateAlert
- ClimateAlert 1:N Recommendation

## Como rodar com Docker

```bash
docker compose up -d --build
```

Acesse:

```text
http://localhost:8080/swagger
```

## Como rodar localmente

Entre na pasta da API:

```bash
cd src/AgroOrbit.Api
```

Restaure os pacotes:

```bash
dotnet restore
```

Execute:

```bash
dotnet run
```

## Migrations

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Endpoints principais

| Método | Endpoint | Função |
|---|---|---|
| GET | /api/dashboard | Resumo geral |
| GET | /api/users | Listar usuários |
| POST | /api/users | Criar usuário |
| GET | /api/farms | Listar fazendas |
| POST | /api/farms | Criar fazenda |
| GET | /api/crop-areas | Listar talhões |
| POST | /api/crop-areas | Criar talhão |
| GET | /api/sensors | Listar sensores |
| POST | /api/sensors | Criar sensor |
| GET | /api/sensor-readings | Listar leituras |
| POST | /api/sensor-readings | Registrar leitura |
| GET | /api/satellite-data | Listar dados satelitais |
| POST | /api/satellite-data | Registrar dados orbitais |
| GET | /api/climate-alerts | Listar alertas |
| POST | /api/climate-alerts | Criar alerta |
| PUT | /api/climate-alerts/{id}/resolve | Resolver alerta |
| GET | /api/recommendations | Listar recomendações |
| POST | /api/recommendations | Criar recomendação |

## Regra de negócio principal

A API calcula o risco do talhão usando:

- NDVI médio
- temperatura de superfície
- cobertura de nuvens
- foco de calor próximo

Exemplo:

```text
NDVI baixo + temperatura alta = risco de seca
Foco de calor + temperatura alta = risco crítico
Cobertura de nuvens alta = baixa confiabilidade
```

## Como explicar na apresentação

A AgroOrbit em .NET é uma API administrativa que centraliza os dados agrícolas da solução. Ela permite cadastrar fazendas, talhões e sensores, registrar dados de satélite e gerar alertas automáticos para apoiar o produtor rural.

A API utiliza Entity Framework Core com PostgreSQL, possui relacionamentos 1:N, validações, DTOs, Swagger e arquitetura organizada em camadas.

## Diferencial

O diferencial do projeto é conectar o agronegócio com a Economia Espacial, usando dados orbitais como NDVI e focos de calor para gerar decisões práticas no campo.


## GITHUB

```text
https://github.com/Wiclif06/GSDotNet.git
```