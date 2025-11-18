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

# Verify Everything
dotnet build
```