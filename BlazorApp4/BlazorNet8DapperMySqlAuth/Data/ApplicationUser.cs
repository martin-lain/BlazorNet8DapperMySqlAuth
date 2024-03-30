using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace BlazorNet8DapperMySqlAuth.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            SecurityStamp = Guid.NewGuid().ToString();
        }

       
        public ApplicationUser(string userName) : this()
        {
            UserName = userName;
        }
    }

}
