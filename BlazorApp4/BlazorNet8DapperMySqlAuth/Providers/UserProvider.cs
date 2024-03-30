using BlazorNet8DapperMySqlAuth.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace BlazorNet8DapperMySqlAuth.Providers;

public class UserProvider(DapperContext dapperContext)
    : IUserProvider
{
    public async Task CreateUser(ApplicationUser user)
    {       
        using var connection = dapperContext.Connection;
        
        user.Id = await connection.InsertAsync(user);
    }
    
    public async Task UpdateUser(ApplicationUser user)
    {
        using var connection = dapperContext.Connection;
        
        await connection.UpdateAsync(user);
    }
    
    public async Task<ApplicationUser> GetUserById(int id)
    {
        using var connection = dapperContext.Connection;
        
        return await connection.GetAsync<ApplicationUser>(id);
    }
    
    public async Task<ApplicationUser> GetUserByUserName(string normalizedUserName)
    {
        using var connection = dapperContext.Connection;
        
        return await connection.QueryFirstOrDefaultAsync<ApplicationUser>("SELECT * FROM AspNetUsers WHERE NormalizedUserName = @NormalizedUserName;", 
            new { NormalizedUserName = normalizedUserName });
    }
    
    public async Task<ApplicationUser> GetUserByEmail(string normalizedEmail)
    {
        using var connection = dapperContext.Connection;
        
        return await connection.QueryFirstOrDefaultAsync<ApplicationUser>("SELECT * FROM AspNetUsers WHERE NormalizedEmail = @NormalizedEmail;", new { NormalizedEmail = normalizedEmail });
    }
    
    public async Task DeleteUser(ApplicationUser user)
    {
        using var connection = dapperContext.Connection;
        
        await connection.DeleteAsync(user);
    }
    
}

public interface IUserProvider
{
    Task CreateUser(ApplicationUser user);
    Task UpdateUser(ApplicationUser user);
    Task<ApplicationUser> GetUserById(int id);
    Task<ApplicationUser> GetUserByUserName(string normalizedUserName);
    Task<ApplicationUser> GetUserByEmail(string normalizedEmail);
    Task DeleteUser(ApplicationUser user);
}