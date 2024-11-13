using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhiskyBLL;
using WhiskyBLL.Interfaces;

namespace WhiskyDAL
{
  public class WhiskyRepository : IWhiskyRepository
  {
    private readonly string connectionString;

    public WhiskyRepository(IConfiguration configuration)
    {
      connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new ArgumentNullException("DefaultConnection", "Connection string 'DefaultConnection' is missing.");
    }

    public void CreateWhisky(WhiskyDto whisky)
    {
      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand cmd = new("INSERT INTO whiskys (name, age, year, country, region) VALUES (@name, @age, @year, @country, @region)", conn);
        cmd.Parameters.AddWithValue("@name", whisky.Name);
        cmd.Parameters.AddWithValue("@age", whisky.Age);
        cmd.Parameters.AddWithValue("@year", whisky.Year);
        cmd.Parameters.AddWithValue("@country", whisky.Country);
        cmd.Parameters.AddWithValue("@region", whisky.Region);

        cmd.ExecuteNonQuery();

        conn.Close();
      }
    }

    public List<WhiskyDto> GetWhiskys()
    {
      List<WhiskyDto> whiskys = [];

      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand  cmd = new MySqlCommand ("SELECT * FROM whiskys", conn);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
          WhiskyDto whisky = new()
          {
            Id = reader.GetInt32("id"),
            Name = reader.GetString("name"),
            Age = reader.GetInt32("age"),
            Year = reader.GetInt32("year"),
            Country = reader.GetString("country"),
            Region = reader.GetString("region")
          };
          whiskys.Add(whisky);
        }

        conn.Close();
      }

      return whiskys;
    }

    public WhiskyDto GetWhiskyById(int id)
    {
      WhiskyDto whisky = new();

      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand cmd = new("SELECT * FROM whiskys WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
          whisky.Id = reader.GetInt32("id");
          whisky.Name = reader.GetString("name");
          whisky.Age = reader.GetInt32("age");
          whisky.Year = reader.GetInt32("year");
          whisky.Country = reader.GetString("country");
          whisky.Region = reader.GetString("region");
        }

        conn.Close();
      }

      return whisky;
    }

    public async Task UpdateWhisky(WhiskyDto whisky)
    {
      using (MySqlConnection conn = new(connectionString))
      {
        await conn.OpenAsync();

        MySqlCommand cmd = new("UPDATE whiskys SET name = @name, age = @age, year = @year, country = @country, region = @region WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", whisky.Id);
        cmd.Parameters.AddWithValue("@name", whisky.Name);
        cmd.Parameters.AddWithValue("@age", whisky.Age);
        cmd.Parameters.AddWithValue("@year", whisky.Year);
        cmd.Parameters.AddWithValue("@country", whisky.Country);
        cmd.Parameters.AddWithValue("@region", whisky.Region);

        await cmd.ExecuteNonQueryAsync();

        await conn.CloseAsync();
      }
    }

    public async Task DeleteWhisky(int id)
    {
      using (MySqlConnection conn = new(connectionString))
      {
        await conn.OpenAsync();

        MySqlCommand cmd = new("DELETE FROM whiskys WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        await cmd.ExecuteNonQueryAsync();

        await conn.CloseAsync();
      }
    }
  }
}