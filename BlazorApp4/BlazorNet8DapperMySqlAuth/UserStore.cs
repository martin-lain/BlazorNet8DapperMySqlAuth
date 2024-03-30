using System.Security.Claims;
using BlazorNet8DapperMySqlAuth.Data;
using BlazorNet8DapperMySqlAuth.Providers;
using Dapper;
using Microsoft.AspNetCore.Identity;

namespace BlazorNet8DapperMySqlAuth;

public class UserStore(IUserProvider userProvider, IUserRolesProvider userRolesProvider, IUserClaimsProvider userClaimsProvider,
    IUserTokenProvider userTokenProvider, IUserLoginsProvider userLoginsProvider)
    : IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>, IUserClaimStore<ApplicationUser>, IUserSecurityStampStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>, IUserTwoFactorStore<ApplicationUser>, IUserLockoutStore<ApplicationUser>, IUserAuthenticationTokenStore<ApplicationUser>, IUserAuthenticatorKeyStore<ApplicationUser>, 
        IUserTwoFactorRecoveryCodeStore<ApplicationUser>, IUserLoginStore<ApplicationUser>
{

    public void Dispose()
    {
        // Nothing to dispose
    }

    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    { 
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        await userProvider.CreateUser(user);
        
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        await userProvider.UpdateUser(user);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        await userProvider.DeleteUser(user);
        
        return IdentityResult.Success;
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await userProvider.GetUserById(int.Parse(userId));
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await userProvider.GetUserByUserName(normalizedUserName);
    }

    public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return await userProvider.GetUserByEmail(normalizedEmail);
    }

    public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.PasswordHash is not null);
    }

    public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roleName);
        
        await userRolesProvider.AddToRole(user, roleName);
    }

    public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roleName);
        
        await userRolesProvider.RemoveFromRole(user, roleName);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        return await userRolesProvider.GetRoles(user);
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roleName);
        
        return (await userRolesProvider.GetRoles(user)).Contains(roleName);
    }

    public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleName);
        
        return await userRolesProvider.GetUsersInRole(roleName);
    }

    public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        return await userClaimsProvider.GetClaims(user);
    }

    public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claims);

        await userClaimsProvider.AddClaim(user, claims);
    }

    public async Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claim);
        ArgumentNullException.ThrowIfNull(newClaim);
        
        await userClaimsProvider.ReplaceClaim(user, claim, newClaim);
    }

    public async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(claims);
        
        await userClaimsProvider.RemoveClaims(user, claims);
    }

    public async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(claim);

        return (await userClaimsProvider.GetUsersForClaim(claim));
    }

    public Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.SecurityStamp = stamp;
        return Task.CompletedTask;
    }

    public Task<string?> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(user.SecurityStamp);
    }

    public Task SetPhoneNumberAsync(ApplicationUser user, string? phoneNumber, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.PhoneNumber = phoneNumber;
        return Task.CompletedTask;
    }

    public async Task<string?> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.PhoneNumber;
    }

    public async Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.PhoneNumberConfirmed;
    }

    public async Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.PhoneNumberConfirmed = confirmed;
    }

    public async Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.TwoFactorEnabled = enabled;
    }

    public async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.TwoFactorEnabled;
    }

    public async Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.LockoutEnd;
    }

    public async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.LockoutEnd = lockoutEnd;
    }

    public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.AccessFailedCount++;
        return user.AccessFailedCount;
    }

    public async Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.AccessFailedCount = 0;
    }

    public async Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.AccessFailedCount;
    }

    public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return user.LockoutEnabled;
    }

    public async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        user.LockoutEnabled = enabled;
    }

    public async Task SetTokenAsync(ApplicationUser user, string loginProvider, string name, string? value,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(loginProvider);
        ArgumentNullException.ThrowIfNull(name);
        
        await userTokenProvider.SetToken(user, loginProvider, name, value);
    }

    public async Task RemoveTokenAsync(ApplicationUser user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(loginProvider);
        ArgumentNullException.ThrowIfNull(name);
        
        await userTokenProvider.RemoveToken(user, loginProvider, name);
    }

    public async Task<string?> GetTokenAsync(ApplicationUser user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(loginProvider);
        ArgumentNullException.ThrowIfNull(name);
        
        return await userTokenProvider.GetToken(user, loginProvider, name);
    }

    public async Task SetAuthenticatorKeyAsync(ApplicationUser user, string key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetAuthenticatorKeyAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task ReplaceCodesAsync(ApplicationUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(recoveryCodes);
        
        await userTokenProvider.ReplaceCodes(user, recoveryCodes);
    }

    public async Task<bool> RedeemCodeAsync(ApplicationUser user, string code, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(code);

        var codes = (await userTokenProvider.GetRecoveryCodes(user)).ToList();
        if (codes.Contains(code))
        {
            await ReplaceCodesAsync(user, codes.Where(c => c != code), cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<int> CountCodesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        return (await userTokenProvider.GetRecoveryCodes(user)).Count();
    }

    public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(login);
        
        await userLoginsProvider.AddLogin(user, login);
    }

    public async Task RemoveLoginAsync(ApplicationUser user, string loginProvider, string providerKey,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(loginProvider);
        ArgumentNullException.ThrowIfNull(providerKey);
        
        await userLoginsProvider.RemoveLogin(user, loginProvider, providerKey);
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        return (await userLoginsProvider.GetLogins(user));
    }

    public async Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(loginProvider);
        ArgumentNullException.ThrowIfNull(providerKey);
        
        return (await userLoginsProvider.FindByLogin(loginProvider, providerKey));
    }
}