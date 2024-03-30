using BlazorNet8DapperMySqlAuth.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace BlazorNet8DapperMySqlAuth.Providers;

public class UserLoginsProvider(DapperContext dapperContext)
    : IUserLoginsProvider
{
    public async Task AddLogin(ApplicationUser user, UserLoginInfo login)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync(
            "INSERT INTO AspNetUserLogins (LoginProvider, ProviderKey, ProviderDisplayName, UserId) VALUES (@LoginProvider, @ProviderKey, @ProviderDisplayName, @UserId);",
            new
            {
                LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey,
                ProviderDisplayName = login.ProviderDisplayName, UserId = user.Id
            });
    }

    public async Task RemoveLogin(ApplicationUser user, string loginProvider, string providerKey)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync(
            "DELETE FROM AspNetUserLogins WHERE UserId = @UserId AND LoginProvider = @LoginProvider AND ProviderKey = @ProviderKey;",
            new { UserId = user.Id, LoginProvider = loginProvider, ProviderKey = providerKey });
    }

    public async Task<IList<UserLoginInfo>> GetLogins(ApplicationUser user)
    {
        using var connection = dapperContext.Connection;
        return (await connection.QueryAsync<UserLoginInfo>("SELECT * FROM AspNetUserLogins WHERE UserId = @UserId;",
            new { UserId = user.Id })).AsList();
    }

    public async Task<ApplicationUser?> FindByLogin(string loginProvider, string providerKey)
    {
        using var connection = dapperContext.Connection;
        var user = await connection.QueryFirstOrDefaultAsync<ApplicationUser>(
            "SELECT * FROM AspNetUsers WHERE Id IN (SELECT UserId FROM AspNetUserLogins WHERE LoginProvider = @LoginProvider AND ProviderKey = @ProviderKey);",
            new { LoginProvider = loginProvider, ProviderKey = providerKey });
        return user;
    }
}

public interface IUserLoginsProvider
{
    Task AddLogin(ApplicationUser user, UserLoginInfo login);
    Task RemoveLogin(ApplicationUser user, string loginProvider, string providerKey);
    Task<IList<UserLoginInfo>> GetLogins(ApplicationUser user);
    Task<ApplicationUser?> FindByLogin(string loginProvider, string providerKey);
}