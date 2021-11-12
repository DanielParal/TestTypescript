using System;
using System.Threading.Tasks;
using IS.Helpers.Functions;
using IS.Models.Dtos.User;
using IS.Models.Options;
using IS.Services.Authorization;
using IS.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace IS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public AccountController(IAuthService authService, IUserService userService, IOptions<AppSettings> options)
        {
            _authService = authService;
            _userService = userService;
            _appSettings = options.Value;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        {
            try
            {
                var userDto = await _authService.Login(userForLoginDto.Username, userForLoginDto.Password);

                if (userDto == null)
                {
                    return Unauthorized();
                }

                var accessToken = _authService.CreateAccessToken(userDto.Guid, userDto.Email);
                var refreshToken = _authService.CreateRefreshToken();

                await _userService.UpdateUsersRefreshToken(userDto.Guid, refreshToken);

                var cookieOptions = new CookieOptions()
                    {HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict};
                // set the access jwt token to the cookie
                Response.Cookies.Append(_appSettings.TokenCookieName, accessToken, cookieOptions);

                // set the refresh token to the cookie
                Response.Cookies.Append(_appSettings.RefreshTokenCookieName, refreshToken, cookieOptions);

                return Ok(new
                {
                    accessTokenPayload = Base64HelperFunctions.Base64Decode(_authService.GetAccessTokenPayload(accessToken))
                });
            }
            catch (Exception ex)
            {
                // log the exception
                return Unauthorized();
            }

        }


    }
}
