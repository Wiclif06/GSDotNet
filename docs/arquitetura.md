# Arquitetura da API AgroOrbit

```text
Cliente / Swagger / App
        ↓
Controllers
        ↓
DTOs de entrada
        ↓
Services
        ↓
Entity Framework Core
        ↓
PostgreSQL
```

## Camadas

- Controllers: recebem requisições HTTP.
- DTOs: validam e transportam dados.
- Services: concentram regras de negócio.
- Data: contexto do banco.
- Domain: entidades e enums.
- Middlewares: tratamento global de exceções.
