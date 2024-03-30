using System.Security.Claims;
using BlazorNet8DapperMySqlAuth.Data;
using BlazorNet8DapperMySqlAuth.Models;
using Dapper;

namespace BlazorNet8DapperMySqlAuth.Providers;

public class UserClaimsProvider(DapperContext dapperContext)
    : IUserClaimsProvider
{
    public async Task<IList<Claim>> GetClaims(ApplicationUser user)
    {
        using var connection = dapperContext.Connection;
        var userClaims = (await connection.QueryAsync<UserClaim>(
            "SELECT * FROM AspNetUserClaims WHERE UserId = @UserId;",
            new { UserId = user.Id }));
        
        return userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)).ToList();
    }

    public async Task AddClaim(ApplicationUser user, IEnumerable<Claim> claims)
    {
        using var connection = dapperContext.Connection;
        using var transaction = connection.BeginTransaction();
        try
        {
            foreach (var claim in claims)
            {
                await connection.ExecuteAsync(
                    "INSERT INTO AspNetUserClaims (UserId, ClaimType, ClaimValue) VALUES (@UserId, @ClaimType, @ClaimValue);",
                    new { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task ReplaceClaim(ApplicationUser user, Claim claim, Claim newClaim)
    {
        using var connection = dapperContext.Connection;
        await connection.ExecuteAsync(
            "UPDATE AspNetUserClaims SET ClaimType = @NewClaimType, ClaimValue = @NewClaimValue WHERE UserId = @UserId AND ClaimType = @ClaimType AND ClaimValue = @ClaimValue;",
            new
            {
                UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value, NewClaimType = newClaim.Type,
                NewClaimValue = newClaim.Value
            });
    }

    public async Task RemoveClaims(ApplicationUser user, IEnumerable<Claim> claims)
    {
        using var connection = dapperContext.Connection;
        foreach (var claim in claims)
        {
            await connection.ExecuteAsync(
                "DELETE FROM AspNetUserClaims WHERE UserId = @UserId AND ClaimType = @ClaimType AND ClaimValue = @ClaimValue;",
                new { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
        }
    }

    public async Task<IList<ApplicationUser>> GetUsersForClaim(Claim claim)
    {
        ArgumentNullException.ThrowIfNull(claim);
        
        using var connection = dapperContext.Connection;
        return (await connection.QueryAsync<ApplicationUser>("SELECT * FROM AspNetUsers WHERE Id IN (SELECT UserId FROM AspNetUserClaims WHERE ClaimType = @ClaimType AND ClaimValue = @ClaimValue);", new { ClaimType = claim.Type, ClaimValue = claim.Value })).AsList();

    }
}

public interface IUserClaimsProvider
{
    Task<IList<Claim>> GetClaims(ApplicationUser user);
    Task AddClaim(ApplicationUser user, IEnumerable<Claim> claim);
    Task ReplaceClaim(ApplicationUser user, Claim claim, Claim newClaim);
    Task RemoveClaims(ApplicationUser user, IEnumerable<Claim> claims);
    Task<IList<ApplicationUser>> GetUsersForClaim(Claim claim);
}