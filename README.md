# Apex Legends Buddy Web API

Web API project to support the Apex Legends Buddy Mobile Companion Application.
This project is build using .NET Core, Entity Framework, SQL Server, and Other.

### Run Application

```
# Install Packages
dotnet add package HtmlAgilityPack --version 1.11.58
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.1
dotnet add package Microsoft.EntityFrameworkCore.SQLServer --version 8.0.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.1

# Initialize and Update database via Entity Framework
dotnet ef migrations add InitialMigration
dotnet ef database update
dotnet ef database remove

# Run database (SQL Server) on Docker
docker compose down && docker compose build && docker compose up -d

# Run Application
dotnet watch
```

---

### ToDo's (Features)

- JWT Based
- JWT Refresh
- Docker and Docker-Compose
- Unit Test
- AutoMapper?
- .env file Support
- Move this Readme information to another file
- CronJob (Hangfire?)

### ToDo's (Endpoints)

Coming soon...
