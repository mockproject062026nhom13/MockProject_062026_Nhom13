namespace CRUDAccountDemo.API.Controllers;

using Asp.Versioning;
using CRUDAccountDemo.API.Common;
using CRUDAccountDemo.Business.Constants;
using CRUDAccountDemo.Business.DTOs;
using CRUDAccountDemo.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/accounts")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    /// <summary>
    /// Gets a paginated list of all accounts.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = AccountConstants.MinPage,
        [FromQuery] int pageSize = AccountConstants.DefaultPageSize)
    {
        var result = await accountService.GetAllAsync(page, pageSize);

        var totalPages = (int)Math.Ceiling((double)result.TotalItems / result.PageSize);
        var pagination = new PaginationInfo(
            result.Page,
            result.PageSize,
            totalPages,
            result.TotalItems,
            result.Page < totalPages,
            result.Page > 1);

        var response = new ApiResponse<IEnumerable<AccountResponse>>(
            true, StatusCodes.Status200OK, "Accounts retrieved successfully.",
            result.Items, null, pagination);

        return Ok(response);
    }

    /// <summary>
    /// Gets an account by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var account = await accountService.GetByIdAsync(id);

        var response = new ApiResponse<AccountResponse>(
            true, StatusCodes.Status200OK, "Account retrieved successfully.",
            account, null, null);

        return Ok(response);
    }

    /// <summary>
    /// Creates a new account.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        var account = await accountService.CreateAsync(request);

        var response = new ApiResponse<AccountResponse>(
            true, StatusCodes.Status201Created, "Account created successfully.",
            account, null, null);

        return CreatedAtAction(nameof(GetById), new { id = account.Id }, response);
    }

    /// <summary>
    /// Updates an existing account's full name and email.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountRequest request)
    {
        var account = await accountService.UpdateAsync(id, request);

        var response = new ApiResponse<AccountResponse>(
            true, StatusCodes.Status200OK, "Account updated successfully.",
            account, null, null);

        return Ok(response);
    }

    /// <summary>
    /// Deletes an account by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await accountService.DeleteAsync(id);

        var response = new ApiResponse<object?>(
            true, StatusCodes.Status200OK, "Account deleted successfully.",
            null, null, null);

        return Ok(response);
    }

    /// <summary>
    /// Deposits money into an account.
    /// </summary>
    [HttpPost("{id:guid}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] DepositRequest request)
    {
        var account = await accountService.DepositAsync(id, request);

        var response = new ApiResponse<AccountResponse>(
            true, StatusCodes.Status200OK, "Deposit completed successfully.",
            account, null, null);

        return Ok(response);
    }

    /// <summary>
    /// Withdraws money from an account.
    /// </summary>
    [HttpPost("{id:guid}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] WithdrawRequest request)
    {
        var account = await accountService.WithdrawAsync(id, request);

        var response = new ApiResponse<AccountResponse>(
            true, StatusCodes.Status200OK, "Withdrawal completed successfully.",
            account, null, null);

        return Ok(response);
    }
}
