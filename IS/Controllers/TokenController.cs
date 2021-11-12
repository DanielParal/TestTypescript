using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using IS.Helpers.Extensions;
using IS.Models.Dtos.Token;
using IS.Services.Authorization;
using IS.Services.User;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public TokenController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(TokenToRefreshDto tokenToRefresh)
        {
            try
            {
                if (tokenToRefresh is null)
                {
                    return BadRequest();
                }

                var accessToken = tokenToRefresh.AccessToken;
                var refreshToken = tokenToRefresh.RefreshToken;

                var principal = _authService.ValidateAccessToken(accessToken);
                var userGuid = principal.GetUserGuid();

                if (userGuid == null)
                {
                    return BadRequest();
                }

                var user = await _userService.GetUserDtoByGuid(userGuid);

                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return BadRequest();
                }

                var newAccessToken = _authService.CreateAccessToken(principal.Claims);
                var newRefreshToken = _authService.CreateRefreshToken();

                await _userService.UpdateUsersRefreshToken(user.Guid, newRefreshToken);

                return Ok(new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                // log the exception
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            var userGuid = User.GetUserGuid();
            if (userGuid == null)
            {
                return BadRequest();
            }

            var user = await _userService.GetUserDtoByGuid(userGuid);

            if (user == null)
            {
                return BadRequest();
            }

            await _userService.ResetUsersRefreshToken(user.Guid);

            return NoContent();
        }

        [HttpGet]
        [Route("xsrftoken")]
        [AllowAnonymous]
        public IActionResult XsrfToken()
        {
            return Ok();
        }
    }
}
