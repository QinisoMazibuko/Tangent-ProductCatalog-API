# Tangent Product Catalog API

## Overview

The **Tangent Product Catalog API** is a Proof of Concept (POC) designed to showcase a layered architecture approach for an API that manages product data using a caching mechanism. This POC replaces a traditional database with an in memory cache for simplicity, while ensuring easy migration to more complex data stores like relational or NoSQL databases in future extensions.

### Architectural Decisions

- **Layered Architecture:**
  The application is divided into distinct layers:

  - **API Layer:** Exposes endpoints for product-related operations.
  - **Domain Layer:** Contains core business entities and models.
  - **Services Layer:** Implements business logic, interacting with repositories via interfaces.
  - **Repository Layer:** Responsible for data persistence and retrieval, currently using a basic cache as a data store.

  Each layer has its own unit test project to ensure proper separation of concerns and maintainability.

- **Unit Testing:**
  Each layer is designed with unit testing in mind. By isolating concerns, the tests ensure each layer operates as expected without side effects, especially when interacting with external systems like the cache.

- **Dependency Injection:**
  The repository layer is injected into the service layer using interfaces. This allows for easy swapping of the cache for a database or any other data store without changing the service layer. Each of the llayers have their own IoC extension for clear seperation of dependencies, This also simplifies future scaling or changes.

- **Cache-Based Data Store:**
  For the purpose of this POC, **LazyCache** is used as a simple cache-based data store. This demonstrates how the repository pattern can be decoupled from a database, making it easier to transition to a real database if needed in future implementations.

- **Containerization with Docker:**
  Docker is used to containerize the API, ensuring consistency across environments. This makes it easy to run the API in different environments (development, staging, production) without worrying about discrepancies in system configurations.

- **Elegant API Bootstrapping:**
  API initialization is handled via `StartupExtensions`, a custom startup extension pattern that groups and manages service registrations, middleware, and other bootstrapping logic in a clean and modular fashion.

- **Custom Middleware:**

  - **AuthorizationMiddleware:** Responsible for verifying JWT tokens to authenticate requests.
  - **ExceptionMiddleware:** Handles exceptions globally, providing consistent error responses to API consumers.

  These custom middlewares ensure that security and error handling are centrally managed and reusable across different endpoints.

- **JWT Token Cryptography:**
  A `SubscriptionContext` is used to manage JWT token cryptography, including retrieving the current subscriber from the token for authorization purposes. This ensures that secure and authenticated access is enforced for protected endpoints.

- **Configuration Management with Secure Storage (Future Plan):**
  In the future, secure storage mechanisms such as **Azure Key Vault** or other app settings management services will be implemented to securely store sensitive configuration values such as connection strings, API keys, and JWT signing keys. This will allow sensitive data to be accessed securely without hardcoding it into the application or configuration files. Using a key vault also enables easy rotation and management of secrets, improving the overall security posture of the API.

  This will involve:

  - **Azure Key Vault (or AWS Secrets Manager):** To store sensitive data such as JWT signing keys and API credentials.
  - **App Configuration Services:** To centrally manage non-sensitive configuration data, ensuring that configuration settings can be dynamically adjusted without requiring code redeployment.

  These mechanisms will enhance the security, flexibility, and scalability of the API by keeping sensitive data out of source control and making it easier to manage in dynamic cloud environments.

### Technologies Used

- **Docker:** For containerizing the API, ensuring it runs consistently across different environments.
- **XUnit:** For unit testing the API and its layers (API, Services, Domain, Repository).
- **AutoMapper:** For mapping Data Transfer Objects (DTOs) to domain entities and vice versa.
- **LazyCache:** Used to store and retrieve products in-memory as part of the repository layer.
- **FluentValidation (Planned):** Will be used to validate API requests for products, ensuring data integrity before processing.

### Future Considerations and Extensibility

This architecture is designed with scalability and flexibility in mind:

- **Adding a Database:** The current cache-based repository can easily be swapped out for a real database like SQL Server or MongoDB. The only changes required would be implementing a database repository that adheres to the same interface contract as the current cache-based repository.
- **Scaling:** By containerizing the API with Docker, scaling horizontally (adding more instances) is straightforward. We can leverage orchestration tools like Kubernetes to manage scaling and load balancing across multiple instances.

- **Validation:** As mentioned, **FluentValidation** will be introduced to handle validation of API requests for products, further ensuring data integrity and correctness.

## Getting Started

### Prerequisites

- **Docker Desktop:** Ensure Docker is installed on your machine. You can download it from [Docker's official website](https://www.docker.com/get-started).
- **.NET SDK (optional):** If you plan to run or modify the API outside of Docker, install the .NET SDK 8.0 from [Microsoft's .NET download page](https://dotnet.microsoft.com/download).

### Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/QinisoMazibuko/Tangent-ProductCatalog-API.git
   cd tangent-product-catalog-api

   ```

2. **Build and Run the API in Docker:**

   First, make sure you are in the root directory where the Dockerfile and `docker-compose.yml` are located.

   Run the following command to build and start the API and Redis cache:

   ```bash
   docker compose up --build
   ```

   - This command will:
     Build the API Docker image.
     Pull the Redis image if it hasn't been downloaded yet.
     Start both services.

   The API will be accessible at:

   ```bash
   http://localhost:8080
   ```

3. **Access Swagger UI:**

   The API provides a built-in Swagger UI for testing and documentation. Once the services are up and running, you can access the Swagger UI at:

   ```bash
   http://localhost:8080/swagger

   ```

   - This interface allows you to easily test the API endpoints by providing detailed information about each route and allowing you to make requests directly from your browser.

4. **Run Tests (Optional):**

If you have the .NET SDK installed locally and want to run the unit tests, navigate to the tests/ directory where the unit test projects are located and run the following command:

```bash
dotnet test

```

- This will execute all the unit tests across the different layers (API, Services, Domain, Repository) to ensure the system behaves as expected.

5. **Stopping the Services:**

When you’re done with the API, you can stop and remove the Docker containers by running:

```bash
 docker compose down

```

- This will stop all running services and clean up the containers.

### Extending the API

The **Tangent Product Catalog API** has been built with flexibility and scalability in mind, allowing for easy future enhancements. Below are a few ways the API can be extended:

- **Adding a Database:**
  Currently, the repository layer interacts with a cache (in memory Lazy Cache) for storage as a quick Proof of Concept (POC). To extend the system for production use, a real database such as SQL Server, PostgreSQL, or MongoDB can be added. By adhering to the repository pattern, the service layer depends on an interface, allowing us to swap the current caching repository with a database repository without impacting the service or API layers. This enables smooth transitions or additions of persistent data stores as the system evolves.

- **Scaling Horizontally:**
  Since the API is containerized using Docker, scaling horizontally is straightforward. This can be done by deploying the API in a container orchestration platform such as Kubernetes. Kubernetes or Docker Swarm can manage multiple instances of the API across different nodes, ensuring load balancing, fault tolerance, and high availability. This is essential for handling increased traffic as the API scales to support more users or larger data loads.

- **Implementing Additional Caching Layers:**
  Beyond the current in-memory cache, additional caching layers can be added. For instance, we can implement a distributed cache with Redis to ensure data is shared across multiple instances of the API. This is particularly useful when scaling horizontally, as it avoids data duplication and enhances performance by reducing database load as well as API response times.

- **Security Enhancements:**
  The API currently uses JWT (JSON Web Token) for authentication. This can be extended to include additional security features such as OAuth2, OpenID Connect, or integration with external identity providers like Google, Azure AD, or Auth0. These changes will enhance authentication and authorization capabilities, making the API more secure for use in production environments.

- **Validation with FluentValidation (Planned):**
  FluentValidation will be added to ensure that the incoming requests to the API are thoroughly validated before being processed. This addition will improve data integrity by enforcing strong validation rules on input models, reducing the risk of invalid data causing issues downstream.

- **API Versioning:**
  As the API evolves, We can introduce versioning to ensure backward compatibility with existing clients while allowing for new features and endpoints to be developed. Versioning strategies such as URL-based, query string, or header-based versioning can be implemented to manage different versions of the API simultaneously.

- **Asynchronous Processing:**
  If the API needs to handle long-running processes, introducing background jobs and asynchronous task processing with a message broker (such as RabbitMQ, Azure Service Bus, or AWS SQS) can be beneficial. This would allow the API to offload heavy tasks and process them in the background without blocking API requests, enhancing performance and user experience.

- **Event-Driven Architecture:**
  For larger systems, We may extend the API to use an event-driven architecture with event sourcing. This will allow the API to publish events to a message broker when certain actions occur (e.g., product creation, updates), enabling other systems to subscribe to these events and react accordingly. This approach ensures decoupled systems and scalable architectures that can handle complex workflows.
