using IS.Models.Dtos.User;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IS.Services.Authorization
{
    public interface IAuthService
    {
        public string CreateAccessToken(string userId, string userName);
        public string CreateAccessToken(IEnumerable<Claim> claims);
        public ClaimsPrincipal ValidateAccessToken(string jwtToken, bool validateLifeTime = true);
        public string CreateRefreshToken();
        public Task<UserDto> Login(string email, string password);
        public string GetAccessTokenPayload(string accessToken);
    }
}
