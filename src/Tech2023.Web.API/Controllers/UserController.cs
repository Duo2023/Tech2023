using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tech2023.DAL;
using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.API.Controllers;

/// <summary>
/// The users API controller, responsible for registering/signing in and various other auth activites
/// </summary>
[Route("api/[controller]")]
[ApiController]
public sealed class UserController : ControllerBase
{
#nullable disable
    internal readonly UserManager<ApplicationUser> _userManager;
    internal readonly RoleManager<ApplicationRole> _roleManager;
    internal readonly IClaimsService _claimsService;
    internal readonly IJwtTokenService _jwtTokenService;
#nullable restore

    internal static string GetCtorErrorMessage(string objectName)
    {
        return $"Object '{objectName}' is null in {nameof(UserController)} ctor, this is likely because the DI services haven't been added";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class
    /// </summary>
    public UserController(
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager,
        IClaimsService claimsService,
        IJwtTokenService jwtTokenService)
    {
        Debug.Assert(userManager != null, GetCtorErrorMessage(nameof(userManager)));
        Debug.Assert(roleManager != null, GetCtorErrorMessage(nameof(roleManager)));
        Debug.Assert(claimsService != null, GetCtorErrorMessage(nameof(claimsService)));

        _userManager = userManager;
        _roleManager = roleManager;
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
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
            return Conflict(AuthResult.Fail(new string[]
            {
                "Register payload was found empty"
            }));
        }

        if (register.Password != register.ConfirmPassword)
        {
            return Conflict(AuthResult.Fail(new string[]
            {
                "Password should be the same as confirm password."
            }));
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

        return CreatedAtAction(ApiRoutes.Users.Register, AuthResult.Ok());
    }
}
