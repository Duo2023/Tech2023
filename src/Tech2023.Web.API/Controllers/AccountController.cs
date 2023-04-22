﻿using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Tech2023.DAL;
using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Authentication;
using Tech2023.Web.Shared.Email;

namespace Tech2023.Web.API.Controllers;

/// <summary>
/// The users API controller, responsible for registering/signing in and various other auth activites
/// </summary>
[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public sealed class AccountController : ControllerBase
{
#nullable disable
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly RoleManager<ApplicationRole> _roleManager;
    internal readonly IClaimsService _claimsService;
    internal readonly IJwtTokenService _jwtTokenService;
    internal readonly IEmailClient _emailClient;
    internal readonly IOptions<JwtOptions> _options;
#nullable restore

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string GetCtorErrorMessage(string objectName)
    {
        return $"Object '{objectName}' is null in {nameof(AccountController)} ctor, this is likely because the DI services haven't been added";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController"/> class
    /// </summary>
    public AccountController(
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager,
        IClaimsService claimsService,
        IJwtTokenService jwtTokenService,
        IEmailClient emailClient,
        IOptions<JwtOptions> options)
    {
        Debug.Assert(userManager != null, GetCtorErrorMessage(nameof(userManager)));
        Debug.Assert(roleManager != null, GetCtorErrorMessage(nameof(roleManager)));
        Debug.Assert(claimsService != null, GetCtorErrorMessage(nameof(claimsService)));
        Debug.Assert(emailClient != null, GetCtorErrorMessage(nameof(emailClient)));


        _userManager = userManager;
        _roleManager = roleManager;
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
        _emailClient = emailClient;
        _options = options;
    }

    [HttpPost]
    [Route(ApiRoutes.Users.Login)]
    public async Task<IActionResult> LoginAsync([FromBody] Login login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);

        if (user is null || await _userManager.CheckPasswordAsync(user, login.Password))
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = _jwtTokenService.GetJwtToken(claims);
        var refreshToken = AuthHelper.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_options.Value.RefreshTokenValid);

        await _userManager.UpdateAsync(user);

        return Ok();
    }


    [HttpPost]
    [Route(ApiRoutes.Users.Register)]
    public async Task<IActionResult> RegisterAsync(Register register)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (await _userManager.FindByEmailAsync(register.Email) is not null)
        {
            return Conflict();
        }

        ApplicationUser user = new()
        {
            Email = register.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var result = await _userManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost]
    [Route(ApiRoutes.Users.ForgotPassword)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPasswordAsync([FromQuery] string email)
    {
        if (email is null)
        {
            return BadRequest();
        }

        await _emailClient.SendEmailAsync(email, "Password Reset Request", "TODO");

        return NoContent();
    }
}
