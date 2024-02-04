### Needs

- Web Api
- JWT Based
- JWT Refresh
- Docker and Docker-Compose
- Unit Test
- AutoMapper?
- .env file Support

### Commands

```
dotnet add package HtmlAgilityPack --version 1.11.58

dotnet add package Microsoft.EntityFrameworkCore --version 8.0.1
dotnet add package Microsoft.EntityFrameworkCore.SQLServer --version 8.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.1

dotnet ef migrations add InitialMigration
dotnet ef database update
dotnet ef database remove

dotnet watch

docker compose down && docker compose build && docker compose up -d

docker build -t chrsolr-api .
```

### ToDo's

Coming soon...
