using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        IEmailClient emailClient)
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
    }


    /// <summary>
    /// The register action registers a user for use of the application
    /// </summary>
    [HttpPost]
    [Route(ApiRoutes.Users.Register)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAsync([FromBody] Register register)
    {
        if (register == null)
        {
            return Conflict(GetJsonAuthResult(AuthResult.Fail(new string[]
            {
                "Register payload was found empty"
            })));
        }

        if (register.Password != register.ConfirmPassword)
        {
            return Conflict(GetJsonAuthResult(AuthResult.Fail(new string[]
            {
                "Password should be the same as confirm password."
            })));
        }

        IdentityResult result;

        ApplicationUser user = new()
        {
            Email = register.Email,
            UserName = register.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        result = await _userManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
        {
            return Conflict(AuthResult.Fail(result.Errors.Select(error => error.Description)));
        }

        return CreatedAtAction(ApiRoutes.Users.Register, GetJsonAuthResult(AuthResult.Ok()));
    }

    /// <summary>
    /// Endpoint for a user to login to
    /// </summary>
    [HttpPost]
    [Route(ApiRoutes.Users.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] Login login)
    {
        if (login is null)
        {
            return BadRequest(GetJsonAuthResult(AuthResult.Fail(new string[]
            {
                "Login body was found empty"
            })));
        }

        if (string.IsNullOrWhiteSpace(login.Email))
        {
            return BadRequest(GetJsonAuthResult(AuthResult.Fail(new string[]
            {
                "Email was empty"
            })));
        }

        var user = await _userManager.FindByEmailAsync(login.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, login.Password))
        {
            return Unauthorized(AuthResult.Fail(new string[]
            {
                "The email and password combination is invalid"
            }));
        }

        var claims = await _claimsService.GetUserClaimsAsync(user);

        var token = _jwtTokenService.GetJwtToken(claims);

        return Ok(LoginResult.Ok(new TokenObject()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        }));
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

    /// <summary>
    /// Gets a JSON text auth result
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string GetJsonAuthResult(AuthResult result) => JsonSerializer.Serialize(result, WebSerializationContext.Default.AuthResult);
}
