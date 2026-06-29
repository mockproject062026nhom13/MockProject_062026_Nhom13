# CLAUDE.md

# Project Overview

This project is a simple backend application built with **ASP.NET Core .NET 8**.

The application provides basic account management features:

* Create account
* Read account
* Update account
* Delete account
* Deposit money
* Withdraw money

This project is intentionally designed as a learning project and must remain simple.

---

# Important Documentation

Before generating, modifying, or refactoring code, always read and follow:

```text
BE-CODING-RULES.md
BE-ARCHITECTURE.md
```

These files define the project's coding conventions and architecture decisions.

---

# Architecture

This project uses a simple **3-Layer Architecture**:

```text
CRUDAccountDemo.API
            ↓
CRUDAccountDemo.Business
            ↓
CRUDAccountDemo.Data
```

Responsibilities:

* API layer handles HTTP requests and responses.
* Business layer contains business logic.
* Data layer handles database access.

Do not introduce additional architectural layers unless explicitly requested.

---

# Architectural Constraints

Do NOT introduce:

* Clean Architecture
* Domain-Driven Design (DDD)
* CQRS
* MediatR
* Event Sourcing
* Domain Events
* Microservices
* Message Brokers
* Unit of Work pattern
* Generic Repository pattern
* Specification pattern
* Event Bus
* Background Jobs
* Distributed Transactions

This project intentionally favors simplicity over architectural complexity.

---

# Coding Rules

Always follow the conventions defined in:

```text
BE-CODING-RULES.md
```

Important rules include:

* Use .NET 8 features when appropriate.
* Prefer file-scoped namespaces.
* One type per file.
* Use PascalCase for types and methods.
* Use camelCase for local variables and parameters.
* Use `_camelCase` for private readonly fields.
* Use Primary Constructor for dependency injection.
* Prefer DTO records.
* Prefer `var` when the type is obvious.
* Avoid magic numbers.
* Write comments in English.

---

# Business Rules

Current business rules:

### Account

* Email must be unique.
* Deposit amount must be greater than zero.
* Withdrawal amount must be greater than zero.
* Account balance cannot become negative.

---

# Entity Framework Core

* Use Entity Framework Core.
* Use SQL Server.
* Use Repository pattern for data access.
* Do not access DbContext directly from API controllers.
* Business logic must not exist inside repositories.

---

# API Response Format

All APIs should return the standardized response format:

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

Validation errors:

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

---

# Error Handling

Use centralized exception handling middleware.

Prefer custom exceptions such as:

```text
BusinessException
ValidationException
NotFoundException
```

Avoid using try-catch blocks in controllers.

---

# Code Generation Rules

When generating code:

* Prefer simple solutions.
* Prefer readability over abstraction.
* Avoid unnecessary interfaces.
* Avoid unnecessary inheritance hierarchies.
* Avoid premature optimization.
* Avoid over-engineering.
* Follow the existing project structure.
* Generate production-quality code.

When multiple solutions exist, always choose the simplest solution that satisfies the requirements.

---

# Guiding Principles

Always follow:

* KISS (Keep It Simple, Stupid)
* YAGNI (You Aren't Gonna Need It)
* SOLID (when appropriate)
* Separation of Concerns (SoC)

The primary goal of this project is:

> Build a simple, maintainable, and understandable backend application while learning fundamental backend development concepts.
