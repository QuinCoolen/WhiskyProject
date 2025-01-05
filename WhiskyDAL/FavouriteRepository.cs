using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using WhiskyDAL.Entities;

namespace WhiskyDAL
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly string connectionString;

        public FavouriteRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                              ?? throw new ArgumentNullException("DefaultConnection", "Connection string 'DefaultConnection' is missing.");
        }

        public void AddFavourite(FavouriteDto favourite)
        {
            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new("INSERT INTO favourites (user_id, whisky_id) VALUES (@userId, @whiskyId)", conn);
                cmd.Parameters.AddWithValue("@userId", favourite.UserId);
                cmd.Parameters.AddWithValue("@whiskyId", favourite.WhiskyId);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public List<FavouriteDto> GetFavouritesByUserId(int userId)
        {
            List<FavouriteDto> favourites = new();

            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new("SELECT * FROM favourites WHERE user_id = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    FavouriteDto favourite = new()
                    {
                        UserId = reader.GetInt32("user_id"),
                        WhiskyId = reader.GetInt32("whisky_id")
                    };
                    favourites.Add(favourite);
                }

                conn.Close();
            }

            return favourites;
        }

        public void RemoveFavourite(int userId, int whiskyId)
        {
            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new("DELETE FROM favourites WHERE user_id = @userId AND whisky_id = @whiskyId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@whiskyId", whiskyId);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
} 