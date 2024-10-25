# MediatR.Extensions

This repository contains a collection of extensions and utilities for the MediatR library, including behaviors, abstractions, and ASP.NET Core integrations.  
The repository is organized into multiple projects, each serving a specific purpose.

## Projects

### MediatR.Extensions.Behaviours
This project contains various pipeline behaviors for MediatR, such as validation and response verification behaviors.  
These behaviors can be used to add additional processing steps to MediatR requests and responses.

### MediatR.Extensions.Abstractions
This project provides common abstractions and interfaces used by other projects in the repository.  
It includes interfaces for validation errors and response validation.

### MediatR.Extensions.AspNetCore
This project contains extensions for integrating MediatR with ASP.NET Core applications.  
It includes methods for converting MediatR results to HTTP responses.

### Analyzers
The `Analyzers` directory contains several projects related to Roslyn analyzers and code fixes for MediatR extensions. These projects help enforce best practices and provide code fixes for common issues.

- **MediatR.Extensions.Analyzers**: Contains the main analyzer logic.
- **MediatR.Extensions.Analyzers.CodeFixes**: Contains code fixes for issues detected by the analyzers.
- **MediatR.Extensions.Analyzers.Package**: Packages the analyzers and code fixes into a NuGet package.
- **MediatR.Extensions.Analyzers.Test**: Contains unit tests for the analyzers and code fixes.
- **MediatR.Extensions.Analyzers.Vsix**: Contains a Visual Studio extension (VSIX) for the analyzers and code fixes.

### WebApplication2
This directory contains a sample ASP.NET Core web application that demonstrates the usage of the MediatR extensions provided in this repository.

## Building and Running the Projects

To build and run the projects in this repository, follow these steps:

1. Clone the repository:
   ```sh
   git clone https://github.com/dileepsam/MediatR.Extensions.git
   cd MediatR.Extensions
   ```

2. Build the solution:
   ```sh
   dotnet build
   ```

## License

This repository is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.
