using Application.DTOs.UserDtos;
using Application.UseCases.UserUseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Users.Controllers;

[ApiController]
[Route("Api/Users")]
public class UserController : ControllerBase
{
    private readonly ChangePasswordUseCase _changePassword;
    private readonly ConfirmEmailUseCase _confirmEmail;
    private readonly GetCurrentUserUseCase _getCurrentUser;
    private readonly LoginUseCase _login;
    private readonly LogOutUseCase _logout;
    private readonly RecoverPasswordUseCase _recoverPassword;
    private readonly RegisterUseCase _register;
    private readonly DeleteUserUseCase _deleteUser;
    private readonly UpdateUserUseCase _updateUser;
    public UserController(UpdateUserUseCase updateUser,DeleteUserUseCase deleteUser,ChangePasswordUseCase changePassword, ConfirmEmailUseCase confirmEmail, GetCurrentUserUseCase getCurrentUser, LoginUseCase login, LogOutUseCase logout, RecoverPasswordUseCase  recoverPassword, RegisterUseCase register)
    {
        _updateUser = updateUser;
        _deleteUser = deleteUser;
        _changePassword = changePassword;
        _confirmEmail = confirmEmail;
        _getCurrentUser = getCurrentUser;
        _login = login;
        _logout = logout;
        _recoverPassword = recoverPassword;
        _register = register;
    }

    [HttpPatch("updateUser")]
    [Authorize(Policy = "ActiveUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest dto)
    {
        var currentUser = await _getCurrentUser.Execute();

        var currentUserDto = new CurrentUserDto
        {
            Id = Guid.Parse(currentUser.Value!.Id!),
            Role = currentUser.Value.Role!,
            IsActive = currentUser.Value.IsActive
        };

        await _updateUser.UpdateUserAsync(dto, currentUserDto, dto.IsActive);

        return NoContent();
    }

    [HttpDelete("deleteUser")]
    [Authorize(Roles = "admin", Policy = "ActiveUser")]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest dto)
    {
        await _deleteUser.DeleteUser(dto.email!);
        return NoContent();
    }

    [HttpGet("users/{id}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetUser([FromRoute] string id)
    {
        var result = await _getCurrentUser.Execute();
        if (result.Value!.Id == id) return Ok(result.Value);

        return Forbid($"no puedes ver la informacion del usuario con Id {result.Value.Id}");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var result = await _login.Login(dto);
        if (result.IsSuccess) return Ok();
        if (result.Error == "You have to confirm your email to continue") return RedirectToAction(nameof(SendConfirmEmail), new {userId = result.Value});
        return BadRequest(result.Error);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        var result = await _register.Register(dto);
        if (result.IsSuccess) return Created($"/users/{result.Value!.Id}",result.Value);

        return BadRequest(result.Error);
    }

    [HttpGet("confirmEmail/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> SendConfirmEmail([FromRoute]string userId)
    {
        var result = await _confirmEmail.SendConfirmEmail(userId);
        if (result.IsSuccess) return Ok(result.Message); 

        return BadRequest(result.Error);
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        token = Uri.UnescapeDataString(token);
        await _confirmEmail.ConfirmEmail(userId, token);
        return Ok("email confirmado");
    }

    [HttpGet("recoverPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest dto)
    {
        var result = await _recoverPassword.RecoverPassword(dto.email);
        if (!result.IsSuccess) return BadRequest(result.Error);

        return Ok(result.Message);
    }

    [HttpPatch("changePassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromQuery] string userId, [FromQuery] string token,[FromBody] ChangePasswordDto dto) 
    {
        var result = await _changePassword.ChangePassword(userId,token,dto); 
        if (!result.IsSuccess) return BadRequest(result.Error);

        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "user", Policy = "ActiveUser")]
    public async Task<IActionResult> Logout()
    {
        await _logout.LogOut();
        return Ok();
    }
}