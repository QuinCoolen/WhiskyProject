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

    public void CreateWhisky(WhiskyDTO whisky)
    {
    }

    public List<WhiskyDTO> GetWhiskys()
    {
      List<WhiskyDTO> whiskys = [];

      using (MySqlConnection conn = new(connectionString))
      {
        conn.Open();

        MySqlCommand  cmd = new MySqlCommand ("SELECT * FROM whiskys", conn);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
          WhiskyDTO whisky = new()
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

    public WhiskyDTO GetWhiskyById(int id)
    {
      return null;
    }

    public async Task UpdateWhisky(WhiskyDTO whisky)
    {

    }

    public async Task DeleteWhisky(int id)
    {

    }
  }
}