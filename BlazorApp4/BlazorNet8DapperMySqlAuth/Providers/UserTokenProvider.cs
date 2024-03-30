using BlazorNet8DapperMySqlAuth.Data;
using Dapper;

namespace BlazorNet8DapperMySqlAuth.Providers;

public class UserTokenProvider(DapperContext dapperContext)
    : IUserTokenProvider
{
    public async Task SetToken(ApplicationUser user, string loginProvider, string name, string? value)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("INSERT INTO AspNetUserTokens (UserId, LoginProvider, Name, Value) VALUES (@UserId, @LoginProvider, @Name, @Value);", new { UserId = user.Id, LoginProvider = loginProvider, Name = name, Value = value });
    }

    public async Task RemoveToken(ApplicationUser user, string loginProvider, string name)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync(
            "DELETE FROM AspNetUserTokens WHERE UserId = @UserId AND LoginProvider = @LoginProvider AND Name = @Name;",
            new { UserId = user.Id, LoginProvider = loginProvider, Name = name });
    }

    public async Task<string?> GetToken(ApplicationUser user, string loginProvider, string name)
    {
        using var connection = dapperContext.Connection;
        return await connection.QueryFirstOrDefaultAsync<string>("SELECT Value FROM AspNetUserTokens WHERE UserId = @UserId AND LoginProvider = @LoginProvider AND Name = @Name;", new { UserId = user.Id, LoginProvider = loginProvider, Name = name });
    }

    public async Task ReplaceCodes(ApplicationUser user, IEnumerable<string> recoveryCodes)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("DELETE FROM AspNetUserTokens WHERE UserId = @UserId AND Name = 'RecoveryCode';", new { UserId = user.Id });
        foreach (var code in recoveryCodes)
        {
            await connection.ExecuteAsync("INSERT INTO AspNetUserTokens (UserId, Name, Value) VALUES (@UserId, 'RecoveryCode', @Value);", new { UserId = user.Id, Value = code });
        }
    }

    public async Task<IEnumerable<string>> GetRecoveryCodes(ApplicationUser user)
    {
        using var connection = dapperContext.Connection;
        return await connection.QueryAsync<string>("SELECT Value FROM AspNetUserTokens WHERE UserId = @UserId AND Name = 'RecoveryCode';", new { UserId = user.Id });
    }
}

public interface IUserTokenProvider
{
    Task SetToken(ApplicationUser user, string loginProvider, string name, string? value);
    Task RemoveToken(ApplicationUser user, string loginProvider, string name);
    Task<string?> GetToken(ApplicationUser user, string loginProvider, string name);
    Task ReplaceCodes(ApplicationUser user, IEnumerable<string> recoveryCodes);
    Task<IEnumerable<string>> GetRecoveryCodes(ApplicationUser user);
}