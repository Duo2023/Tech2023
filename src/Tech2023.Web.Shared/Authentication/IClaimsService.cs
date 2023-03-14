using System.Security.Claims;
using Tech2023.DAL;

namespace Tech2023.Web.Shared.Authentication;

public interface IClaimsService
{
    Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);
}
