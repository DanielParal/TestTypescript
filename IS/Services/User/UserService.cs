using System;
using IS.Models.Dtos.User;
using System.Threading.Tasks;
using IS.Repositories.User;

namespace IS.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserDtoByEmail(string email)
        {
            return await _userRepository.GetUserDtoByEmail(email);
        }

        public async Task<UserDto> GetUserDtoByGuid(string guid)
        {
            return await _userRepository.GetUserDtoByGuid(guid);
        }

        public async Task UpdateUsersRefreshToken(string userGuid, string refreshToken)
        {
            var refreshTokenExpire = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUsersRefreshToken(userGuid, refreshToken, refreshTokenExpire);
        }

        public async Task ResetUsersRefreshToken(string userGuid)
        {
            await _userRepository.UpdateUsersRefreshToken(userGuid, null, null);
        }
    }
}
