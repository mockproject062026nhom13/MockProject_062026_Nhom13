namespace CRUDAccountDemo.Business.DTOs;

public record UpdateAccountRequest(
    string FullName,
    string Email);
