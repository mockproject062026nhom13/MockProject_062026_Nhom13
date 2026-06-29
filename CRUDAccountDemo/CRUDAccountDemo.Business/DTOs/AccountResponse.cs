namespace CRUDAccountDemo.Business.DTOs;

public record AccountResponse(
    Guid Id,
    string FullName,
    string Email,
    decimal Balance,
    DateTime CreatedAt,
    DateTime UpdatedAt);
