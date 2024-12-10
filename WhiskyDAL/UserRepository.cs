using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;

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
      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand cmd = new("INSERT INTO users (name, email, password) VALUES (@name, @email, @password)", conn);
        cmd.Parameters.AddWithValue("@name", user.Name);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);

        cmd.ExecuteNonQuery();

        conn.Close();
      }
    }

    public List<UserDto> GetUsers()
    {
      List<UserDto> users = [];

      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand  cmd = new MySqlCommand ("SELECT * FROM users", conn);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
          UserDto user = new()
          {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("name"),
            Email = reader.GetString("email"),
            Password = reader.GetString("password")
          };
          users.Add(user);
        }
      }

      return users;
    }

    public UserDto GetUserById(int id)
    {
      UserDto user = null;

      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand cmd = new("SELECT * FROM users WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
          user = new()
          {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("name"),
            Email = reader.GetString("email"),
            Password = reader.GetString("password")
          };
        }

        conn.Close();
      }

      return user;
    }

    public async Task UpdateUser(UserDto user)
    {
      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand cmd = new("UPDATE users SET name = @name, email = @email, password = @password WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@name", user.Name);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);
        cmd.Parameters.AddWithValue("@id", user.Id);

        await cmd.ExecuteNonQueryAsync();

        conn.Close();
      }
    }

    public async Task DeleteUser(int id)
    {
      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand cmd = new("DELETE FROM users WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        await cmd.ExecuteNonQueryAsync();

        conn.Close();
      }
    }
  }
}