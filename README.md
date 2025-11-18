# AI-Model-Ordering-App
A modular .NET solution for managing AI models, customers, and orders. Includes Blazor Web UI, console importer, MySQL persistence, and clean layered architecture.

## Project Setup Commands

```shell

cd AI-Model-Ordering-App
ls -lrt # Outputs README.md and LICENSE file

dotnet new sln -n AI-Model-Ordering-App

# Create BASE PROJECTS (no dependencies)
dotnet new classlib -n app-models
dotnet sln add app-models/app-models.csproj

dotnet new classlib -n app-common
dotnet sln add app-common/app-common.csproj

dotnet new classlib -n app-core
dotnet sln add app-core/app-core.csproj
dotnet add app-core/app-core.csproj reference app-models/app-models.csproj


# Create SERVICES layer
dotnet new classlib -n app-services
dotnet sln add app-services/app-services.csproj

# Add references:
dotnet add app-services/app-services.csproj reference app-models/app-models.csproj
dotnet add app-services/app-services.csproj reference app-common/app-common.csproj
dotnet add app-services/app-services.csproj reference app-core/app-core.csproj

# Create CONSOLE application (app-cli)
dotnet new console -n app-cli
dotnet sln add app-cli/app-cli.csproj

# Add references:
dotnet add app-cli/app-cli.csproj reference app-services/app-services.csproj
dotnet add app-cli/app-cli.csproj reference app-models/app-models.csproj
dotnet add app-cli/app-cli.csproj reference app-common/app-common.csproj
dotnet add app-cli/app-cli.csproj reference app-core/app-core.csproj

# Create WEB application (Blazor Server)
dotnet new blazorserver -n app-web
dotnet sln add app-web/app-web.csproj

# Add references:
dotnet add app-web/app-web.csproj reference app-services/app-services.csproj
dotnet add app-web/app-web.csproj reference app-models/app-models.csproj
dotnet add app-web/app-web.csproj reference app-common/app-common.csproj
dotnet add app-web/app-web.csproj reference app-core/app-core.csproj

# DB Related in app-services
dotnet add app-services/app-services.csproj package Dapper
dotnet add app-services/app-services.csproj package MySql.Data

dotnet add app-cli/app-cli.csproj package Microsoft.Extensions.Hosting
dotnet add app-cli/app-cli.csproj package Microsoft.Extensions.DependencyInjection


# Verify Everything
dotnet build

brew install mysql
brew services start mysql

dotnet run --project app-cli/app-cli.csproj \
--service-name customer \
--input-file customers-crud.json

mysql -u root 

CREATE DATABASE AIModelOrdering;

SHOW DATABASES;

CREATE TABLE Customers (
    Id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Email VARCHAR(200) NOT NULL,
    Phone VARCHAR(50) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL
);

SHOW TABLES;

DESCRIBE Customers;

GRANT ALL PRIVILEGES ON AIModelOrdering.* TO 'root'@'localhost';
FLUSH PRIVILEGES;


```