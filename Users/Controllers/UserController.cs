using Application.DTOs.UserDtos;
using Application.UseCases.UserUseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    public UserController(ChangePasswordUseCase changePassword, ConfirmEmailUseCase confirmEmail, GetCurrentUserUseCase getCurrentUser, LoginUseCase login, LogOutUseCase logout, RecoverPasswordUseCase  recoverPassword, RegisterUseCase register)
    {
        _changePassword = changePassword;
        _confirmEmail = confirmEmail;
        _getCurrentUser = getCurrentUser;
        _login = login;
        _logout = logout;
        _recoverPassword = recoverPassword;
        _register = register;
    }

    [HttpGet("users/{id}")]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> GetUser([FromRoute] string id)
    {
        var result = await _getCurrentUser.Execute();
        if (result.Value!.Id == id) return Ok(result.Value);

        return Forbid(result.Error);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto)
    {
        var result = await _login.Login(dto);
        if (result.IsSucces) return Ok();
        if (result.Error == "You have to confirm your email to continue") return RedirectToAction(nameof(ConfirmEmail), new {id = result.Value});
        return BadRequest(result.Error);
    }

    [HttpPut("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
    {
        var result = await _register.Register(dto);
        if (result.IsSucces) return Created($"/users/{result.Value!.Id}",result.Value);

        return BadRequest(result.Error);
    }

    [HttpPost("confirmEmail/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromRoute]int id)
    {
        var result = await _confirmEmail.ConfirmEmail(id);
        if (result.IsSucces) return Ok(result.Message); 

        return BadRequest(result.Error);
    }

    [HttpPost("recoverPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> RecoverPassword([FromBody] string email)
    {
        var result = await _recoverPassword.RecoverPassword(email);
        if (!result.IsSucces) return BadRequest(result.Error);

        return Ok(result.Message);
    }

    [HttpPut("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto) 
    {
        var result = await _changePassword.ChangePassword(dto);
        if (!result.IsSucces) return BadRequest(result.Error);

        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> Logout()
    {
        await _logout.LogOut();
        return Ok();
    }
}