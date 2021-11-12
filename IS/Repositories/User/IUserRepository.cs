
using System;
using System.Threading.Tasks;
using IS.Models.Dtos.User;

namespace IS.Repositories.User
{
    public interface IUserRepository
    {
        public Task<UserDto> GetUserDtoByEmail(string email);
        public Task<UserDto> GetUserDtoByGuid(string guid);
        public Task UpdateUsersRefreshToken(string userGuid, string refreshToken, DateTime? refreshTokenExpireTime);
    }
}
