using Horizons.Core.Auth.Models;

namespace Horizons.Core.Auth.Identity.Interface
{
    public interface IJwtProvider
    {
        string GenerateJwtToken(ApplicationUser user, IList<string> roles);
        bool IsTokenValid(string token);
    }
}
