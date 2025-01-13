using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using WhiskyDAL.Entities;
using WhiskyBLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyBLL.Exceptions.Whisky;

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
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("INSERT INTO whiskys (name, age, year, country, region) VALUES (@name, @age, @year, @country, @region)", conn);
                    cmd.Parameters.AddWithValue("@name", whisky.Name);
                    cmd.Parameters.AddWithValue("@age", whisky.Age);
                    cmd.Parameters.AddWithValue("@year", whisky.Year);
                    cmd.Parameters.AddWithValue("@country", whisky.Country);
                    cmd.Parameters.AddWithValue("@region", whisky.Region);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                throw new WhiskyRepoException("An error occurred while creating the whisky.", ex);
            }
            catch (Exception ex)
            {
                throw new WhiskyRepoException("An unexpected error occurred while creating the whisky.", ex);
            }
        }

        public List<WhiskyDto> GetWhiskys()
        {
            try
            {
                List<WhiskyDto> whiskys = new List<WhiskyDto>();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM whiskys", conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        WhiskyDto whisky = new WhiskyDto
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
            catch (MySqlException ex)
            {
                throw new WhiskyRepoException("An error occurred while retrieving whiskys.", ex);
            }
            catch (Exception ex)
            {
                throw new WhiskyRepoException("An unexpected error occurred while retrieving whiskys.", ex);
            }
        }

        public WhiskyDto GetWhiskyById(int id)
        {
            try
            {
                WhiskyDto whisky = null;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM whiskys WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        whisky = new WhiskyDto
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Age = reader.GetInt32("age"),
                            Year = reader.GetInt32("year"),
                            Country = reader.GetString("country"),
                            Region = reader.GetString("region")
                        };
                    }

                    conn.Close();
                }

                if (whisky == null)
                {
                    throw new NotFoundException($"Whisky with ID {id} was not found.");
                }

                return whisky;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (MySqlException ex)
            {
                throw new WhiskyRepoException($"An error occurred while retrieving the whisky with ID {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new WhiskyRepoException($"An unexpected error occurred while retrieving the whisky with ID {id}.", ex);
            }
        }

        public WhiskyDto GetWhiskyByName(string name)
        {
            try
            {
                WhiskyDto whisky = null;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM whiskys WHERE name = @name", conn);
                    cmd.Parameters.AddWithValue("@name", name);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        whisky = new WhiskyDto
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Age = reader.GetInt32("age"),
                            Year = reader.GetInt32("year"),
                            Country = reader.GetString("country"),
                            Region = reader.GetString("region")
                        };
                    }

                    conn.Close();
                }

                if (whisky == null)
                {
                    throw new NotFoundException($"Whisky with name '{name}' was not found.");
                }

                return whisky;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (MySqlException ex)
            {
                throw new WhiskyRepoException($"An error occurred while retrieving the whisky with name '{name}'.", ex);
            }
            catch (Exception ex)
            {
                throw new WhiskyRepoException($"An unexpected error occurred while retrieving the whisky with name '{name}'.", ex);
            }
        }

        public async Task UpdateWhisky(WhiskyDto whisky)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    MySqlCommand cmd = new MySqlCommand("UPDATE whiskys SET name = @name, age = @age, year = @year, country = @country, region = @region WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", whisky.Id);
                    cmd.Parameters.AddWithValue("@name", whisky.Name);
                    cmd.Parameters.AddWithValue("@age", whisky.Age);
                    cmd.Parameters.AddWithValue("@year", whisky.Year);
                    cmd.Parameters.AddWithValue("@country", whisky.Country);
                    cmd.Parameters.AddWithValue("@region", whisky.Region);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    await conn.CloseAsync();

                    if (rowsAffected == 0)
                    {
                        throw new NotFoundException($"Whisky with ID {whisky.Id} was not found for update.");
                    }
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (MySqlException ex)
            {
                throw new WhiskyRepoException($"An error occurred while updating the whisky with ID {whisky.Id}.", ex);
            }
            catch (Exception ex)
            {
                throw new WhiskyRepoException($"An unexpected error occurred while updating the whisky with ID {whisky.Id}.", ex);
            }
        }

        public async Task DeleteWhisky(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    MySqlCommand cmd = new MySqlCommand("DELETE FROM whiskys WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    await conn.CloseAsync();

                    if (rowsAffected == 0)
                    {
                        throw new NotFoundException($"Whisky with ID {id} was not found for deletion.");
                    }
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (MySqlException ex)
            {
                throw new WhiskyRepoException($"An error occurred while deleting the whisky with ID {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new WhiskyRepoException($"An unexpected error occurred while deleting the whisky with ID {id}.", ex);
            }
        }
    }
}