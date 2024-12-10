using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhiskyBLL.Interfaces;
using WhiskyBLL.Domain;

namespace WhiskyDAL {
  public class UserRepository : IUserRepository
  {
    private readonly string connectionString;

    public UserRepository(IConfiguration configuration)
    {
      connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new ArgumentNullException("DefaultConnection", "Connection string 'DefaultConnection' is missing.");
    }

    public void CreateUser(UserDomain user)
    {
      try {
        using (MySqlConnection conn = new(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new("INSERT INTO users (name, email, password) VALUES (@name, @email, @password)", conn);
          cmd.Parameters.AddWithValue("@name", user.Name);
          cmd.Parameters.AddWithValue("@email", user.Email);
          cmd.Parameters.AddWithValue("@password", user.PasswordHash);

          cmd.ExecuteNonQuery();

          conn.Close();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to create user: " + ex.Message);
      }
    }
  }
}