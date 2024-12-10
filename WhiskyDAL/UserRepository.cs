using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhiskyBLL.Interfaces;
using WhiskyBLL.Dto;

namespace WhiskyDAL {
  public class UserRepository : IUserRepository
  {
    private readonly string connectionString;

    public UserRepository(IConfiguration configuration)
    {
      connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new ArgumentNullException("DefaultConnection", "Connection string 'DefaultConnection' is missing.");
    }

    public void CreateUser(UserDto user)
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

    public UserDto GetUserByEmail(string email)
    {
      try {
        using (MySqlConnection conn = new(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new("SELECT * FROM users WHERE email = @email", conn);
          cmd.Parameters.AddWithValue("@email", email);

          using (MySqlDataReader reader = cmd.ExecuteReader())
          {
            if (reader.Read())
            {
              return new UserDto
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Email = reader.GetString("email"),
                PasswordHash = reader.GetString("password")
              };
            }
          }

          conn.Close();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Failed to retrieve user: " + ex.Message);
      }

      return null;
    }
  }
}