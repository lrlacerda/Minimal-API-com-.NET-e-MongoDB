# Minimal API com .NET e MongoDB

Este projeto demonstra como criar uma API mínima (Minimal API) utilizando .NET e MongoDB. A API permite realizar operações básicas de CRUD (Create, Read, Update, Delete) em um banco de dados MongoDB.

## Tecnologias Utilizadas

- .NET 8
- MongoDB
- MongoDB.Driver (Biblioteca oficial do MongoDB para .NET)

## Requisitos

- .NET 8 SDK
- MongoDB Server

- Endpoints da API
GET /items: Retorna todos os itens.
GET /items/{id}: Retorna um item pelo ID.
POST /items: Cria um novo item.
PUT /items/{id}: Atualiza um item existente pelo ID.
DELETE /items/{id}: Deleta um item pelo ID.
Estrutura do Projeto
Program.cs: Configuração da Minimal API e definição dos endpoints.
appsettings.json: Configurações da aplicação, incluindo a string de conexão com o MongoDB.
Models/Item.cs: Modelo de dados para os itens.
Services/ItemService.cs: Serviço para interação com o MongoDB.
