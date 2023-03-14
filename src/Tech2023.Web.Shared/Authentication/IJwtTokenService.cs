using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Tech2023.Web.Shared;

public interface IJwtTokenService
{
    JwtSecurityToken GetJwtToken(List<Claim> userClaims);
}
