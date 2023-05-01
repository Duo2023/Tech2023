using System.Diagnostics;
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

// https://code-maze.com/using-refresh-tokens-in-asp-net-core-authentication/

/// <summary>
/// The users API controller, responsible for registering/signing in and various other auth activites
/// </summary>
[Route(Defaults.Controller)]
[Produces(Defaults.ContentType)]
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
    [Route(ApiRoutes.Users.Refresh)]
    public async Task<IActionResult> RefreshAsync(Token token)
    {
        if (token is null)
        {
            return BadRequest();
        }

        string accessToken = token.AccessToken;
        string refreshToken = token.RefreshToken;

        var principal = new ClaimsPrincipal();

        if (principal is null)
        {
            return BadRequest();
        }

        string username = principal.Identity!.Name!;

        var user = await _userManager.FindByNameAsync(username);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest();
        }

        var newAccessToken = "";
        var newRefreshToken = AuthHelper.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;

        await _userManager.UpdateAsync(user);

        return Ok(new Token()
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
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
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] EmailAddress address)
    {
        if (await _userManager.FindByEmailAsync(address.Email) == null)
        {
            return NoContent(); // return the same thing regardless of whether it exists, its still an attack vector
        }

        await _emailClient.SendEmailAsync(address.Email, "Password Reset Request", "TODO");

        return NoContent();
    }
}
