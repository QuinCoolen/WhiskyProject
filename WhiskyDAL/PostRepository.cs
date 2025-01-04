using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using WhiskyDAL.Entities;

namespace WhiskyDAL
{
    public class PostRepository : IPostRepository
    {
        private readonly string connectionString;

        public PostRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                              ?? throw new ArgumentNullException("DefaultConnection", "Connection string 'DefaultConnection' is missing.");
        }

        public void CreatePost(PostDto post)
        {
            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new("INSERT INTO posts (user_id, whisky_id, description, rating, datetime) VALUES (@userId, @whiskyId, @description, @rating, @datetime)", conn);
                cmd.Parameters.AddWithValue("@userId", post.UserId);
                cmd.Parameters.AddWithValue("@whiskyId", post.WhiskyId);
                cmd.Parameters.AddWithValue("@description", post.Description);
                cmd.Parameters.AddWithValue("@rating", post.Rating);
                cmd.Parameters.AddWithValue("@datetime", post.CreatedAt);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public List<PostDto> GetPosts()
        {
            List<PostDto> posts = new();

            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new("SELECT * FROM posts", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PostDto post = new()
                    {
                        Id = reader.GetInt32("id"),
                        UserId = reader.GetInt32("user_id"),
                        WhiskyId = reader.GetInt32("whisky_id"),
                        Description = reader.GetString("description"),
                        Rating = reader.GetInt32("rating"),
                        CreatedAt = reader.GetDateTime("datetime")
                    };
                    posts.Add(post);
                }

                conn.Close();
            }

            return posts;
        }

        public PostDto GetPostById(int id)
        {
            PostDto post = null;

            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new("SELECT * FROM posts WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    post = new PostDto
                    {
                        Id = reader.GetInt32("id"),
                        UserId = reader.GetInt32("user_id"),
                        WhiskyId = reader.GetInt32("whisky_id"),
                        Description = reader.GetString("description"),
                        Rating = reader.GetInt32("rating"),
                        CreatedAt = reader.GetDateTime("datetime")
                    };
                }

                conn.Close();
            }

            return post;
        }

        public void UpdatePost(PostDto post)
        {
            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new("UPDATE posts SET user_id = @userId, whisky_id = @whiskyId, description = @description, rating = @rating, datetime = @datetime WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", post.Id);
                cmd.Parameters.AddWithValue("@userId", post.UserId);
                cmd.Parameters.AddWithValue("@whiskyId", post.WhiskyId);
                cmd.Parameters.AddWithValue("@description", post.Description);
                cmd.Parameters.AddWithValue("@rating", post.Rating);
                cmd.Parameters.AddWithValue("@datetime", post.CreatedAt);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void DeletePost(int id)
        {
            using (MySqlConnection conn = new(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new("DELETE FROM posts WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
} 