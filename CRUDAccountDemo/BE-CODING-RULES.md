# BE-CODING-RULES.md

# Backend Coding Rules

## General

| Category  | Rule          |
| --------- | ------------- |
| Framework | .NET 8        |
| IDE       | VS Code 1.126 |

---

# Namespace

| Rule                  | Description                                                     |
| --------------------- | --------------------------------------------------------------- |
| File-Scoped Namespace | Prefer file-scoped namespace syntax.                            |
| Namespace Naming      | Namespace must reflect the folder structure and use PascalCase. |

### Example

```csharp
namespace CRUDAccountDemo.Application.Services;
```

---

# File Structure

| Rule              | Description                                               |
| ----------------- | --------------------------------------------------------- |
| One Type Per File | Each file must contain only one class, record, or struct. |

### Good

```text
User.cs
CreateUserRequest.cs
UpdateUserRequest.cs
```

### Bad

```text
User.cs
├── User
├── UserDto
└── UserResponse
```

---

# Class / Record / Struct

| Rule              | Description                |
| ----------------- | -------------------------- |
| Naming Convention | Use PascalCase.            |
| Naming Style      | Use nouns or noun phrases. |

### Examples

```csharp
public class User
{
}

public record CreateUserRequest
{
}

public struct Money
{
}
```

---

# Interface

| Rule              | Description                         |
| ----------------- | ----------------------------------- |
| Naming Convention | Use PascalCase with the `I` prefix. |

### Examples

```csharp
public interface IUserRepository
{
}

public interface IAccountService
{
}
```

---

# Method

| Rule              | Description                                            |
| ----------------- | ------------------------------------------------------ |
| Naming Convention | Use PascalCase and start with a verb.                  |
| Async Methods     | Asynchronous methods must end with the `Async` suffix. |

### Examples

```csharp
public User GetUserById(Guid id)
{
}

public Task<User> GetUserAsync(Guid id)
{
}
```

---

# Variables

| Rule            | Description    |
| --------------- | -------------- |
| Local Variables | Use camelCase. |
| Parameters      | Use camelCase. |

### Example

```csharp
var accountBalance = 1000m;

public void Deposit(decimal amount)
{
}
```

---

# Fields

| Rule                    | Description       |
| ----------------------- | ----------------- |
| Private Readonly Fields | Use `_camelCase`. |

### Example

```csharp
private readonly IUserRepository _userRepository;
private readonly ILogger<AccountService> _logger;
```

---

# Properties

| Rule              | Description     |
| ----------------- | --------------- |
| Naming Convention | Use PascalCase. |

### Example

```csharp
public Guid Id { get; set; }

public string FullName { get; set; } = string.Empty;
```

---

# DTO

| Rule   | Description                                      |
| ------ | ------------------------------------------------ |
| Usage  | Prefer DTOs when needed.                         |
| Naming | DTO names should clearly indicate their purpose. |
| Type   | DTOs should use `record`.                        |

### Examples

```csharp
public record CreateUserRequest;

public record LoginResponse;

public record UserDto;
```

---

# Dependency Injection

| Rule                  | Description                                          |
| --------------------- | ---------------------------------------------------- |
| Constructor Injection | Prefer Primary Constructor for dependency injection. |

### Example

```csharp
public class UserService(
    IUserRepository userRepository,
    ILogger<UserService> logger)
{
}
```

---

# Constants

| Rule              | Description                              |
| ----------------- | ---------------------------------------- |
| Naming Convention | Use PascalCase.                          |
| Security          | Never store sensitive data in constants. |

### Example

```csharp
public const int MaximumRetryCount = 3;
```

---

# Secrets

| Rule           | Description                                                                                                     |
| -------------- | --------------------------------------------------------------------------------------------------------------- |
| Sensitive Data | Secret keys, connection strings, and sensitive URLs must be loaded from `.env` or secure configuration sources. |

---

# Collections

| Rule                   | Description                              |
| ---------------------- | ---------------------------------------- |
| Collection Expressions | Prefer Collection Expressions.           |
| Spread Operator        | Prefer Spread Operator when appropriate. |

### Examples

```csharp
List<int> numbers = [1, 2, 3];

List<int> allNumbers =
[
    ..oddNumbers,
    ..evenNumbers
];
```

---

# Arrays

| Rule         | Description                                                             |
| ------------ | ----------------------------------------------------------------------- |
| Empty Arrays | Prefer `[]` over `Array.Empty<T>()` when aligned with team conventions. |

### Example

```csharp
string[] names = [];
```

---

# var

| Rule  | Description                            |
| ----- | -------------------------------------- |
| Usage | Prefer `var` when the type is obvious. |

### Example

```csharp
var user = new User();
var numbers = new List<int>();
```

---

# Switch

| Rule  | Description                                              |
| ----- | -------------------------------------------------------- |
| Usage | Prefer Switch Expressions over simple switch statements. |

### Example

```csharp
var status = code switch
{
    200 => "Success",
    404 => "Not Found",
    _ => "Unknown"
};
```

---

# Magic Numbers

| Rule  | Description                                        |
| ----- | -------------------------------------------------- |
| Usage | Do not hard-code values. Define them as constants. |

### Bad

```csharp
if (retryCount > 3)
{
}
```

### Good

```csharp
private const int MaximumRetryCount = 3;

if (retryCount > MaximumRetryCount)
{
}
```

---

# Shared Resources

| Rule              | Description                                                    |
| ----------------- | -------------------------------------------------------------- |
| Shared Components | Shared resources across the project must be managed centrally. |

---

# Comments

| Rule            | Description                                                    |
| --------------- | -------------------------------------------------------------- |
| Language        | All comments must be written in English.                       |
| Line Comments   | Use line comments for short explanations.                      |
| Block Comments  | Use block comments for complex logic or detailed explanations. |
| Method Comments | Methods should include a short description comment.            |

### Examples

```csharp
// Validate account balance
```

```csharp
/*
 * Calculate the monthly interest amount
 * based on the current account balance.
 */
```

```csharp
/// <summary>
/// Retrieves an account by its identifier.
/// </summary>
public Account GetById(Guid id)
{
}
```

---

# TODO / FIXME

| Rule  | Description                                        |
| ----- | -------------------------------------------------- |
| TODO  | Use for future improvements or enhancements.       |
| FIXME | Use for known bugs or issues requiring correction. |

### Examples

```csharp
// TODO: Add caching support
```

```csharp
// FIXME: Handle overflow scenario
```

---

# Folders

| Rule              | Description                       |
| ----------------- | --------------------------------- |
| Naming Convention | Folder names must use PascalCase. |

### Example

```text
Application
Domain
Infrastructure
Persistence
Controllers
Services
Repositories
```
