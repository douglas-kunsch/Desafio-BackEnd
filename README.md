# MotoRent

O **MotoRent** é um sistema desenvolvido em **.NET 8** que segue os princípios de **Clean Architecture**, utilizando **Minimal APIs**, **CQRS** e boas práticas de separação de camadas (Domain, Application, Infrastructure, Api, Consumers, Workers).  

O projeto conta com **mensageria com RabbitMQ**, **armazenamento de arquivos no MinIO**, persistência no **PostgreSQL**, além de **validações com FluentValidation** e **Health Checks** para monitoramento da API.  

Também está incluída uma **collection do Postman** para facilitar os testes da API.  

---

## Tecnologias Utilizadas

- **.NET 8** com **Minimal API**  
- **Clean Architecture** (Domain, Application, Infrastructure, API, Worker)  
- **CQRS** (Command Query Responsibility Segregation)  
- **Entity Framework Core** com **Migrations**  
- **FluentValidation** para regras de negócio  
- **xUnit** para testes automatizados  
- **RabbitMQ** para mensageria (publicação e consumo de eventos)  
- **MinIO** para armazenamento de arquivos (compatível com S3)  
- **PostgreSQL** como banco de dados relacional  
- **Docker Compose** para orquestração dos serviços  
- **Health Checks** expostos na API  

---

## Como rodar em ambiente de desenvolvimento

### 1. Subir os containers necessários
```bash
docker compose up -d
```

> Isso inicia: **PostgreSQL**, **RabbitMQ**, **MinIO**, **API** e **Worker**.  
> A API estará disponível em: [http://localhost:8080](http://localhost:8080)

---

### 2. Aplicar as migrações do banco
Se você ainda não aplicou as migrações:
```bash
cd src/MotoRent.Api
dotnet ef database update
```

---

### 3. Rodar a API manualmente (opcional)
Se preferir rodar fora do Docker:
```bash
cd src/MotoRent.Api
dotnet run
```

Acesse a documentação (Swagger):  
 [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## Health Check
A API possui endpoint de health check para monitorar o status da aplicação e dependências:
```http
GET http://localhost:8080/health
```

---

## Mensageria com RabbitMQ
- Eventos são **publicados** pela API.  
- O **Worker** (consumer) processa mensagens da fila `MotorcycleRegistered`.  
- Exemplo de uso: registrar uma moto publica um evento que é consumido e persistido no banco.

---

## Armazenamento de Arquivos (MinIO)
- Arquivos são enviados para o bucket configurado no MinIO (`motorent-bucket`).  
- O bucket é inicializado automaticamente no `docker compose`.

---

## Testes
O projeto inclui testes com **xUnit**.  
Para rodar:
```bash
dotnet test
```

---

## Postman Collection
Na raiz do projeto você encontrará uma **collection do Postman** (`MotoRent.postman_collection.json`) para testar os endpoints da API rapidamente.

---

## Resumo
O MotoRent combina **boas práticas de arquitetura** com **infraestrutura realista** (mensageria, storage, banco relacional) em um ambiente pronto para desenvolvimento local via Docker.
