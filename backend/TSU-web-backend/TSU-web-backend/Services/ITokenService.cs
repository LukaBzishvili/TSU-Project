using TSU_web_backend.Models;

namespace TSU_web_backend.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
