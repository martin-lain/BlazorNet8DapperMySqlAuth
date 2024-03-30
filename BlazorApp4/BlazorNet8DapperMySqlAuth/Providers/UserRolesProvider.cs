using BlazorNet8DapperMySqlAuth.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace BlazorNet8DapperMySqlAuth.Providers;

public class UserRolesProvider(DapperContext dapperContext)
    : IUserRolesProvider
{
    public async Task AddToRole(ApplicationUser user, string roleName)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roleName);
        
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);", 
            new { UserId = user.Id, RoleId = roleName });
    }

    public async Task RemoveFromRole(ApplicationUser user, string roleName)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roleName);
        
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync("DELETE FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId;", 
            new { UserId = user.Id, RoleId = roleName });
    }

    public async Task<IList<string>> GetRoles(ApplicationUser user)
    {
        using var connection = dapperContext.Connection;
        return (await connection.QueryAsync<string>("SELECT RoleId FROM AspNetUserRoles WHERE UserId = @UserId;", new { UserId = user.Id })).AsList();
    }

    public async Task<bool> IsInRole(ApplicationUser user, string roleName)
    {
        using var connection = dapperContext.Connection;
        return await connection.QueryFirstOrDefaultAsync<bool>("SELECT COUNT(*) FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId;", new { UserId = user.Id, RoleId = roleName });
    }

    public async Task<IList<ApplicationUser>> GetUsersInRole(string roleName)
    {
        using var connection = dapperContext.Connection;
        return (await connection.QueryAsync<ApplicationUser>("SELECT * FROM AspNetUsers WHERE Id IN (SELECT UserId FROM AspNetUserRoles WHERE RoleId = @RoleId);", new { RoleId = roleName })).AsList();
    }
}

public interface IUserRolesProvider
{
    public Task AddToRole(ApplicationUser user, string roleName);
    public Task RemoveFromRole(ApplicationUser user, string roleName);
    public Task<IList<string>> GetRoles(ApplicationUser user);
    public Task<bool> IsInRole(ApplicationUser user, string roleName);
    Task<IList<ApplicationUser>> GetUsersInRole(string roleName);
}