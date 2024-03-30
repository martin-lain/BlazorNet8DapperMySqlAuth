using BlazorNet8DapperMySqlAuth;
using Dapper;
using Microsoft.AspNetCore.Identity;

public class DapperRolesStore(DapperContext dapperContext)
    : IRoleStore<IdentityRole>
{
    public void Dispose()
    {
        // Nothing to dispose
    }

    public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES (@Id, @Name, @NormalizedName);", role);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("UPDATE AspNetRoles SET Name = @Name, NormalizedName = @NormalizedName WHERE Id = @Id;", role);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("DELETE FROM AspNetRoles WHERE Id = @Id;", role);
        return IdentityResult.Success;
    }

    public async Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        return await connection.QueryFirstOrDefaultAsync<string>("SELECT Id FROM AspNetRoles WHERE Name = @Name;", role);
    }

    public async Task<string?> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        return await connection.QueryFirstOrDefaultAsync<string>("SELECT Name FROM AspNetRoles WHERE Id = @Id;", role);
    }

    public async Task SetRoleNameAsync(IdentityRole role, string? roleName, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("UPDATE AspNetRoles SET Name = @Name WHERE Id = @Id;", role);
    }

    public async Task<string?> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        return await connection.QueryFirstOrDefaultAsync<string>("SELECT NormalizedName FROM AspNetRoles WHERE Id = @Id;", role);
    }

    public async Task SetNormalizedRoleNameAsync(IdentityRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("UPDATE AspNetRoles SET NormalizedName = @NormalizedName WHERE Id = @Id;", role);
    }

    public async Task<IdentityRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        return await connection.QueryFirstOrDefaultAsync<IdentityRole>("SELECT * FROM AspNetRoles WHERE Id = @Id;", new { Id = roleId });
    }

    public async Task<IdentityRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        using var connection = dapperContext.Connection;
        return await connection.QueryFirstOrDefaultAsync<IdentityRole>("SELECT * FROM AspNetRoles WHERE NormalizedName = @NormalizedName;", new { NormalizedName = normalizedRoleName });
    }
}