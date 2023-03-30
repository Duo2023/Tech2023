using System.Buffers;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Tech2023.Web.Shared.Authentication;

/// <summary>
/// Issues out JWT security tokens
/// </summary>
public sealed class JwtTokenService : IJwtTokenService
{
    internal readonly IOptions<JwtOptions> _options;
    internal readonly SigningCredentials _creds;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenService"/> class
    /// </summary>
    public JwtTokenService(IOptions<JwtOptions> options)
    {
        Debug.Assert(options != null);

        if (options.Value.Secret is null)
        {
            throw new ConfigurationException("JWT options have not been configured to run this application, see JwtOptions.cs");
        }

        _options = options;
        _creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret)), SecurityAlgorithms.HmacSha256);
    }

    public JwtSecurityToken GetJwtToken(List<Claim> userClaims)
    {
        var token = new JwtSecurityToken(
            issuer: _options.Value.ValidIssuer,
            audience: _options.Value.ValidAudience,
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(_options.Value.ExpiryInMinutes),
            signingCredentials: _creds
            );

        return token;
    }
}
