using System.Data;
using MySqlConnector;

namespace BlazorNet8DapperMySqlAuth;

public class DapperContext(IConfiguration configuration)
{
  
    public IDbConnection Connection => 
        new MySqlConnection(configuration.GetConnectionString("MySqlConnection"));
 
    public IDbConnection MasterConnection => 
        new MySqlConnection(configuration.GetConnectionString("MySqlMasterConnection"));
}