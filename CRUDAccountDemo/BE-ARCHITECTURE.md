# BE-ARCHITECTURE.md

# Backend Architecture

## Overview

This project uses a simple **3-Layer Architecture**.

The goals of this architecture are:

* Keep the codebase simple and easy to understand.
* Separate presentation, business, and data access concerns.
* Improve maintainability and readability.
* Avoid unnecessary architectural complexity.
* Focus on learning and implementing core backend concepts.

This project intentionally does not use:

* Clean Architecture
* Domain-Driven Design (DDD)
* CQRS
* Event Sourcing
* Microservices
* Message Brokers
* Distributed Transactions

---

# Solution Structure

```text
CRUDAccountDemo.sln

├── CRUDAccountDemo.API
├── CRUDAccountDemo.Business
└── CRUDAccountDemo.Data
```

---

# Architecture Diagram

```text
Client
   |
   v
API Layer
   |
   v
Business Layer
   |
   v
Data Layer
   |
   v
Database
```

---

# Dependency Direction

Dependencies must follow this direction:

```text
CRUDAccountDemo.API
            ↓
CRUDAccountDemo.Business
            ↓
CRUDAccountDemo.Data
```

Reverse dependencies are not allowed.

Examples:

❌ Data → Business

❌ Business → API

❌ API → Database directly

---

# 1. API Layer

Project:

```text
CRUDAccountDemo.API
```

Responsibilities:

* Receive HTTP requests.
* Validate request formats.
* Call business services.
* Return API responses.
* Configure dependency injection.
* Configure middleware.
* Configure Swagger.

Example structure:

```text
CRUDAccountDemo.API

├── Controllers/
├── Middlewares/
├── Extensions/
├── Filters/
└── Program.cs
```

Examples:

```text
AccountController
ExceptionHandlingMiddleware
ServiceCollectionExtensions
```

Controllers must not contain business logic.

---

# 2. Business Layer

Project:

```text
CRUDAccountDemo.Business
```

Responsibilities:

* Implement business logic.
* Define business models.
* Define DTOs.
* Validate business rules.
* Coordinate data access operations.
* Define business exceptions.

Example structure:

```text
CRUDAccountDemo.Business

├── Entities/
├── DTOs/
├── Interfaces/
├── Services/
├── Constants/
└── Exceptions/
```

---

## Entities

Entities represent business objects.

Example:

```text
Account
```

Example properties:

```text
Id
Email
Balance
CreatedAt
UpdatedAt
```

---

## DTOs

DTOs are used for communication between API and business layers.

Examples:

```text
CreateAccountRequest
UpdateAccountRequest
DepositRequest
WithdrawRequest
AccountResponse
```

DTOs should use `record`.

---

## Services

Services contain business logic.

Examples:

```text
IAccountService
AccountService
```

Example business rules:

* Email must be unique.
* Deposit amount must be greater than zero.
* Withdrawal amount must be greater than zero.
* Account balance cannot become negative.

Example:

```csharp
public interface IAccountService
{
    Task<AccountResponse> CreateAsync(CreateAccountRequest request);

    Task DepositAsync(Guid accountId, decimal amount);

    Task WithdrawAsync(Guid accountId, decimal amount);
}
```

---

## Exceptions

Custom exceptions are defined in the Business layer.

Examples:

```text
BusinessException
NotFoundException
ValidationException
```

---

# 3. Data Layer

Project:

```text
CRUDAccountDemo.Data
```

Responsibilities:

* Access the database.
* Configure Entity Framework Core.
* Implement repositories.
* Execute CRUD operations.
* Manage migrations.

Example structure:

```text
CRUDAccountDemo.Data

├── DbContexts/
├── Repositories/
├── Configurations/
├── Migrations/
└── Seeders/
```

Examples:

```text
ApplicationDbContext
AccountRepository
AccountConfiguration
DatabaseSeeder
```

---

## Repositories

Repositories are responsible for data access only.

Examples:

```text
IAccountRepository
AccountRepository
```

Responsibilities:

* Query data.
* Insert data.
* Update data.
* Delete data.

Repositories must not contain business logic.

---

# Database Access

This project uses:

```text
ASP.NET Core
Entity Framework Core
SQL Server
```

Entity Framework Core is accessed through the Repository pattern.

Business logic must never access DbContext directly.

---

# Dependency Injection

Dependency injection should use Primary Constructor syntax.

Example:

```csharp
public class AccountService(
    IAccountRepository accountRepository)
    : IAccountService
{
}
```

---

# Error Handling

The application uses centralized exception handling middleware.

Examples:

```text
BusinessException
ValidationException
NotFoundException
```

Controllers should not contain try-catch blocks unless absolutely necessary.

---

# API Response Format

All APIs should return a unified response structure.

Example:

```json
{
    "success": true,
    "statusCode": 200,
    "message": "Operation completed successfully.",
    "data": {},
    "errors": null,
    "pagination": null
}
```

Validation Error example:

```json
{
    "success": false,
    "statusCode": 400,
    "message": "Validation failed.",
    "data": null,
    "errors": [
        {
            "field": "email",
            "message": "Email already exists."
        }
    ],
    "pagination": null
}
```

Paginated list example:

```json
{
    "success": true,
    "statusCode": 200,
    "message": "Accounts retrieved successfully.",
    "data": [],
    "errors": null,
    "pagination": {
        "page": 2,
        "pageSize": 20,
        "totalPages": 15,
        "totalItems": 300,
        "hasNext": true,
        "hasPrevious": true
    }
}
```

`pagination` is only populated for paginated list endpoints. All other endpoints return `"pagination": null`.

---

# Design Principles

This project follows the following principles:

* Separation of Concerns (SoC)
* Single Responsibility Principle (SRP)
* Dependency Injection (DI)
* Keep It Simple, Stupid (KISS)
* You Aren't Gonna Need It (YAGNI)

The primary goals of this architecture are:

* Simplicity
* Readability
* Maintainability
* Learning fundamental backend development concepts

```
```
