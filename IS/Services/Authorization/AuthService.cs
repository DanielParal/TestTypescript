using IS.Models.Dtos.User;
using IS.Models.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IS.Services.User;
using System.Collections.Generic;

namespace IS.Services.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettingsOptions;
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;

        public AuthService(IOptions<AppSettings> appSettingsOptions, 
                            IUserService userService,
                            IPasswordService passwordService)
        {
            _appSettingsOptions = appSettingsOptions.Value;
            _userService = userService;
            _passwordService = passwordService;
        }

        public string CreateAccessToken(string userId, string userName)
        {
            // create claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName)
            };

            var securityToken = CreateSecurityToken(claims);
            return ConvertSecurityTokenToString(securityToken);
        }

        public string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var securityToken = CreateSecurityToken(claims);
            return ConvertSecurityTokenToString(securityToken);
        }

        public ClaimsPrincipal ValidateAccessToken(string jwtToken, bool validateLifeTime = true)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = validateLifeTime;
            validationParameters.ValidateAudience = false;
            validationParameters.ValidateIssuer = false;
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettingsOptions.Token));

            var principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);

            return principal;
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<UserDto> Login(string email, string password)
        {
            var user = await _userService.GetUserDtoByEmail(email);

            if (user == null)
            {
                return null;
            }

            if (_passwordService.VerifyPassword(user.PasswordHashed, password))
            {
                return user;
            }

            return null;
        }

        private SecurityToken CreateSecurityToken(IEnumerable<Claim> claims)
        {

            // create key and hash it (hash string in appsettings) - signing token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettingsOptions.Token));

            // create credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // create description for the token (expiration date etc)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = creds
            };

            // create token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // create token which will be returned to our client
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        private string ConvertSecurityTokenToString(SecurityToken securityToken)
        {
            // create token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(securityToken);
        }

        public string GetAccessTokenPayload(string accessToken)
        {
            var decodedTokens = accessToken.Split('.');
            return decodedTokens[1];
        }
    }
}
