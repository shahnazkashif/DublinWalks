using Microsoft.AspNetCore.Identity;

namespace DublinWalks.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);

    }
}
