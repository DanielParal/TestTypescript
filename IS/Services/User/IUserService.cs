using IS.Models.Dtos.User;
using System.Threading.Tasks;

namespace IS.Services.User
{
    public interface IUserService
    {
        public Task<UserDto> GetUserDtoByEmail(string email);
        public Task<UserDto> GetUserDtoByGuid(string guid);
        public Task UpdateUsersRefreshToken(string userGuid, string refreshToken);
        public Task ResetUsersRefreshToken(string userGuid);
    }
}
