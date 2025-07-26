# C# Vertical Slice and MediatR Template

This repository is a template for building robust and scalable applications using C# with a vertical slice architecture. It leverages MediatR for implementing the CQRS pattern and includes a comprehensive authentication and authorization system.

## Features

*   **Vertical Slice Architecture:** Organizes code by feature, improving modularity and maintainability.
*   **CQRS with MediatR:** Separates read and write operations, leading to a cleaner and more focused design.
*   **Built-in Authentication:** Includes a complete authentication and authorization solution with features like:
    *   User registration with email confirmation
    *   Secure login with JWT (JSON Web Tokens)
    *   Password reset functionality
    *   User and profile management
*   **Entity Framework Core:** Utilizes EF Core for data access, with a clear separation of data models and configurations.
*   **Docker Support:** Comes with `Dockerfile` and `docker-compose.yml` for easy containerization and deployment.
*   **Unit and Integration Testing:** Includes a dedicated test project with examples of unit and integration tests.
*   **Clean and Organized Structure:** The project is structured to be easily understood and extended.

## Technologies Used

*   [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
*   [ASP.NET Core](https://dotnet.microsoft.com/en-us/apps/aspnet)
*   [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
*   [MediatR](https://github.com/jbogard/MediatR)
*   [Docker](https://www.docker.com/)
*   [xUnit](https://xunit.net/)

## Getting Started

### Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop/) (optional, for running with Docker)
*   A database server (e.g., SQL Server, PostgreSQL, SQLite). The project is configured to use SQL Server by default, but you can easily change it in `src/Applications/WebAPI/appsettings.json`.

### Installation

1.  Clone the repository:
    ```bash
    git clone https://github.com/your-username/your-repository.git
    ```
2.  Navigate to the project directory:
    ```bash
    cd your-repository
    ```
3.  Restore the dependencies:
    ```bash
    dotnet restore
    ```
4.  Update the database with the latest migrations:
    ```bash
    dotnet ef database update --project src/Applications/WebAPI
    ```

## Running the Application

### Without Docker

1.  Run the application from the `WebAPI` directory:
    ```bash
    cd src/Applications/WebAPI
    dotnet run
    ```
2.  The API will be available at `https://localhost:5001` (or the port specified in `Properties/launchSettings.json`).

### With Docker

1.  Build and run the application using Docker Compose:
    ```bash
    docker-compose -f docker-compose.dev.yml up -d
    ```
2.  The API will be available at `http://localhost:8080`.

## Running the Tests

To run the tests, navigate to the `test/DemoRepository.Tests` directory and use the `dotnet test` command:

```bash
cd test/DemoRepository.Tests
dotnet test
```

## API Endpoints

The project includes a `WebAPI.http` file with sample requests for the available endpoints. You can use a REST client like [Postman](https://www.postman.com/) or the [REST Client extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) to interact with the API.

### Authentication

*   `POST /api/auth/login`: Authenticates a user and returns a JWT.
*   `POST /api/auth/register`: Registers a new user.
*   `POST /api/auth/forgot-password`: Sends a password reset link to the user's email.
*   `POST /api/auth/reset-password`: Resets the user's password.

### Users

*   `GET /api/users`: Retrieves a list of all users.
*   `GET /api/users/{id}`: Retrieves a specific user by their ID.
*   `POST /api/users`: Creates a new user.

### Profiles

*   `GET /api/profiles`: Retrieves a list of all profiles.
*   `GET /api/profiles/{id}`: Retrieves a specific profile by its ID.
*   `POST /api/profiles`: Creates a new profile.

## Contributing

Contributions are welcome! Please feel free to open an issue or submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
