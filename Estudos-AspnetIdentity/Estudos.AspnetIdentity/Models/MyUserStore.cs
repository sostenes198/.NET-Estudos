using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace Estudos.AspnetIdentity.Models
{
    public class MyUserStore : IUserStore<MyUser>, IUserPasswordStore<MyUser>
    {
        public void Dispose()
        {
        }

        private static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection(@"
                Data Source=SQNOT317\SQLEXPRESS2017;Initial Catalog=EstudosIdentity;user id=sa;password=539624eddas_;MultipleActiveResultSets=true
            ");

            connection.Open();

            return connection;
        }

        public async Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using var connection = GetOpenConnection();
            await connection.ExecuteAsync(
                @"INSERT INTO Users
                       (
                            Id
                           ,UserName
                           ,NormalizedUserName
                           ,PasswordHash
                       )
                 VALUES
                       (
                            @Id
                           ,@UserName
                           ,@NormalizedUserName
                           ,@PasswordHash
                       )
                ",
                user
            );
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using var connection = GetOpenConnection();
            await connection.ExecuteAsync(
                @"
                    UPDATE Users
                    SET  Id = @Id
                        ,UserName = @UserName
                        ,NormalizedUserName = @NormalizedUserName
                        ,PasswordHash = @PasswordHash
                    WHERE Id = @Id
                ",
                user
            );
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            using var connection = GetOpenConnection();
            await connection.ExecuteAsync(
                "delete from EstudosIdentity.dbo.Users where Id = @id",
                new {id = user.Id}
            );
            return IdentityResult.Success;
        }

        public async Task<MyUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using var connection = GetOpenConnection();
            return await connection.QueryFirstOrDefaultAsync<MyUser>(
                "select * from Users where Id = @id",
                new {id = userId}
            );
        }

        public async Task<MyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using var connection = GetOpenConnection();
            return await connection.QueryFirstOrDefaultAsync<MyUser>(
                "select * from Users where normalizedUserName = @name",
                new {name = normalizedUserName}
            );
        }

        public Task<string> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(MyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MyUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != default);
        }

        public Task SetPasswordHashAsync(MyUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
    }
}