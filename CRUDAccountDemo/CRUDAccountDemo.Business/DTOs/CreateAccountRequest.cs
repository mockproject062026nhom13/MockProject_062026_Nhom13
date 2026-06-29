namespace CRUDAccountDemo.Business.DTOs;

public record CreateAccountRequest(
    string FullName,
    string Email,
    decimal InitialBalance = 0);
