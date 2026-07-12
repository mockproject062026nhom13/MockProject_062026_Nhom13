using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingHome.Application.Common;
using NursingHome.Infrastructure.Persistence.DbContexts;
namespace NursingHome.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class ActivateAccountController : ControllerBase
{
    private readonly NursingHomeDbContext _context;

    public ActivateAccountController(NursingHomeDbContext context)
    {
        _context = context;
    }
    //to do: audit log
    [HttpPost("activate-account")]
    public async Task<IActionResult> ActivateAccount([FromBody] ActivateAccountRequest request)
    {
        var errors = new List<ApiError>();

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            errors.Add(new ApiError("email", "Email is required."));
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            errors.Add(new ApiError("password", "Password is required."));
        }

        if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
        {
            errors.Add(new ApiError("confirmPassword", "Confirm password is required."));
        }

        if (!string.IsNullOrWhiteSpace(request.Password)
            && request.Password != request.ConfirmPassword)
        {
            errors.Add(new ApiError("confirmPassword", "Passwords do not match."));
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var hasUpper = request.Password.Any(char.IsUpper);
            var hasLower = request.Password.Any(char.IsLower);
            var hasDigit = request.Password.Any(char.IsDigit);

            if (request.Password.Length < 8 || !hasUpper || !hasLower || !hasDigit)
            {
                errors.Add(new ApiError("password", "Password must contain at least 8 characters, one uppercase letter, one lowercase letter and one number."));
            }
        }

        if (errors.Any())
        {
            return BadRequest(
                ApiResponse<object>.CreateError(
                    400,
                    "Validation failed.",
                    errors));
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Email == request.Email &&
                !x.IsDeleted);

        if (user == null)
        {
            return NotFound(
                ApiResponse<object>.CreateError(
                    404,
                    "User not found."));
        }

        if (user.Status == "Enabled")
        {
            return Conflict(
                ApiResponse<object>.CreateError(
                    409,
                    "Account has already been activated."));
        }
        
        user.ActivateAccount(BCrypt.Net.BCrypt.HashPassword(request.Password), request.PhoneNumber);
        await _context.SaveChangesAsync();

        return Ok(
            ApiResponse<ActivateAccountResponse>.CreateSuccess(
                new ActivateAccountResponse
                {
                    Email = user.Email,
                    Status = user.Status
                },
                200,
                "Account activated successfully."));
    }
}

public class ActivateAccountRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
}

public class ActivateAccountResponse
{
    public string Email { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}
// password: 123456Aa