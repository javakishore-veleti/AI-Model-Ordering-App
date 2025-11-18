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

dotnet new classlib -n app-daos

# Add project references:
dotnet add app-daos/app-daos.csproj reference app-models/app-models.csproj
dotnet add app-daos/app-daos.csproj reference app-core/app-core.csproj

# Install EF Core packages:
dotnet add app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore
dotnet add app-daos/app-daos.csproj package Pomelo.EntityFrameworkCore.MySql
dotnet add app-daos/app-daos.csproj package Microsoft.Extensions.Configuration.Json

dotnet add app-cli/app-cli.csproj reference app-daos/app-daos.csproj


# Verify Everything
dotnet clean
dotnet build

brew install mysql
brew services start mysql

mysql -u root 

CREATE DATABASE AIModelOrdering;

SHOW DATABASES;

USE AIModelOrdering;

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


# If you want consistent predictable IDs each run
mysql -u root

USE AIModelOrdering;

DELETE FROM Customers;
ALTER TABLE Customers AUTO_INCREMENT = 1;


# build in app-web
dotnet add app-web/app-web.csproj reference app-core/app-core.csproj
dotnet add app-web/app-web.csproj reference app-services/app-services.csproj
dotnet add app-web/app-web.csproj reference app-daos/app-daos.csproj
dotnet add app-web/app-web.csproj reference app-models/app-models.csproj

# Remove EF Core 10, install EF Core 8
dotnet remove app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore
dotnet remove app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore.Relational
dotnet remove app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore.Abstractions
dotnet remove app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore.Design
dotnet remove app-daos/app-daos.csproj package Pomelo.EntityFrameworkCore.MySql

dotnet remove app-cli/app-cli.csproj package Microsoft.EntityFrameworkCore

# Install EF Core 8 packages (CORRECT VERSION for .NET 8)
dotnet add app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore --version 8.0.2
dotnet add app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore.Relational --version 8.0.2
dotnet add app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.2
dotnet add app-daos/app-daos.csproj package Pomelo.EntityFrameworkCore.MySql --version 8.0.0

dotnet add app-cli/app-cli.csproj package Microsoft.EntityFrameworkCore --version 8.0.2
dotnet add app-cli/app-cli.csproj package Pomelo.EntityFrameworkCore.MySql --version 8.0.0


dotnet add app-web/app-web.csproj package Microsoft.EntityFrameworkCore --version 8.0.2
dotnet add app-web/app-web.csproj package Pomelo.EntityFrameworkCore.MySql --version 8.0.0
dotnet add app-web/app-web.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.2
dotnet add app-web/app-web.csproj package Microsoft.Extensions.Configuration.Json
dotnet add app-web/app-web.csproj package Microsoft.Extensions.Logging

# Swagger
dotnet add app-web/app-web.csproj package Swashbuckle.AspNetCore --version 6.5.0


dotnet add app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore --version 8.0.2
dotnet add app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore.Relational --version 8.0.2
dotnet add app-daos/app-daos.csproj package Microsoft.EntityFrameworkCore.Design --version 8.0.2
dotnet add app-daos/app-daos.csproj package Pomelo.EntityFrameworkCore.MySql --version 8.0.0

dotnet add app-daos/app-daos.csproj reference app-models/app-models.csproj

dotnet clean
dotnet build

dotnet --list-sdks
dotnet --list-runtimes

dotnet run --project app-cli/app-cli.csproj \
--service-name customer \
--input-file customers-crud.json

dotnet run --project app-cli/app-cli.csproj --no-launch-profile -- \
--service-name customer \
--input-file customers-crud.json \
--program ProgramV2

dotnet clean
dotnet build

dotnet run --project app-web/app-web.csproj

# Open http://localhost:5174/swagger/index.html


# Blazor Implementation
dotnet new blazorserver -n app-blazor

dotnet add app-blazor/app-blazor.csproj reference app-models/app-models.csproj
dotnet add app-blazor/app-blazor.csproj reference app-core/app-core.csproj

# ❗ Do NOT reference app-daos or app-services
# Blazor must call your API, not your EF Core or services directly.

dotnet add app-blazor/app-blazor.csproj package Microsoft.AspNetCore.Components.Web
dotnet add app-blazor/app-blazor.csproj package Microsoft.AspNetCore.Components.WebAssembly

# For calling your API:
dotnet add app-blazor/app-blazor.csproj package System.Net.Http.Json

dotnet clean
dotnet build

dotnet run --project app-blazor/app-blazor.csproj


```

Features also include:

- ✔ Search
- ✔ Pagination
- ✔ Sorting
- ✔ Validation
- ✔ Responsive pages
- ✔ Toast notifications
- ✔ Clean Bootstrap UI


- ✔ Uses ToastService
- ✔ Uses Model instead of Customer parameter
- ✔ Uses OnSubmitSuccess instead of OnSubmit
- ✔ Correct namespace & imports
- ✔ Fully aligned with the improved validation + toasts system


A multi-project .NET solution, plus:

- CLI app ✔
- Web API ✔
- Blazor Server UI ✔
- PostgreSQL/MySQL/DB integration ✔
- Docker builds for all ✔
- Multi-workflow GitHub Actions ✔
- Master orchestrator workflow ✔

