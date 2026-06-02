# Roteiro de vídeo — .NET AgroOrbit

Olá, meu nome é Felipe Wiclif e vou apresentar a entrega de Advanced Business Development with .NET.

A solução desenvolvida é a AgroOrbit, uma API para monitoramento agrícola com dados de satélite, sensores IoT e análise de risco climático.

Nesta API, é possível cadastrar usuários, fazendas, talhões, sensores, leituras de sensores, dados satelitais, alertas e recomendações.

Agora vou mostrar a estrutura do projeto. Temos Controllers, DTOs, Services, Domain, Data e Middlewares.

O banco utilizado é PostgreSQL com Entity Framework Core. Os principais relacionamentos são: usuário com várias fazendas, fazenda com vários talhões, talhão com sensores, talhão com dados satelitais e talhão com alertas.

Agora vou executar o projeto e abrir o Swagger.

No Swagger, vou testar o endpoint de dashboard, listar fazendas e registrar um dado satelital. Quando um dado satelital é cadastrado, a API calcula automaticamente o status do talhão e pode gerar alertas e recomendações.

Com isso, a API atende aos requisitos de REST, banco relacional, relacionamento 1:N, boas práticas de arquitetura, validação, documentação e persistência.
