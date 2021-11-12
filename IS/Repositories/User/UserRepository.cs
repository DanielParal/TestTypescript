using Dapper;
using IS.Models.Dtos.User;
using IS.Services.DbContext;
using System.Threading.Tasks;

namespace IS.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly DbIsContext _dbIscontext;

        public UserRepository(DbIsContext dbIsContext)
        {
            _dbIscontext = dbIsContext;
        }

        public async Task<UserDto> GetUserDtoByEmail(string email)
        {
            var query = "SELECT Guid, Email, Password AS PasswordHashed, RefreshToken, CONVERT(datetime, RefreshTokenExpiryTime) AS RefreshTokenExpiryTime, CONVERT(datetime, DateCreated) AS DateCreated FROM Users WHERE Email = @Email";
            using var connection = _dbIscontext.CreateConnection();
            var user = await connection.QueryFirstOrDefaultAsync<UserDto>(query, new { Email = email });
            return user;
        }

        public async Task<UserDto> GetUserDtoByGuid(string guid)
        {
            var query = "SELECT Guid, Email, Password AS PasswordHashed, RefreshToken, CONVERT(datetime, RefreshTokenExpiryTime) AS RefreshTokenExpiryTime, CONVERT(datetime, DateCreated) AS DateCreated FROM Users WHERE Guid = @Guid";
            using var connection = _dbIscontext.CreateConnection();
            var user = await connection.QueryFirstOrDefaultAsync<UserDto>(query, new { Guid = guid });
            return user;
        }

        public async Task UpdateUsersRefreshToken(string userGuid, string refreshToken, System.DateTime? refreshTokenExpireTime)
        {
            var query = "UPDATE Users SET RefreshToken = @RefreshToken, RefreshTokenExpiryTime = @RefreshTokenExpiryTime WHERE Guid = @UserGuid";
            using var connection = _dbIscontext.CreateConnection();
            await connection.ExecuteAsync(query, new { RefreshToken = refreshToken, RefreshTokenExpiryTime = refreshTokenExpireTime, UserGuid = userGuid });
        }
    }
}
