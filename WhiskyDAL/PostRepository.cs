using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;
using WhiskyDAL.Entities;
using WhiskyBLL.Exceptions;
using System;
using WhiskyBLL.Exceptions.Post;

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
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("INSERT INTO posts (user_id, whisky_id, description, rating, datetime) VALUES (@userId, @whiskyId, @description, @rating, @datetime)", conn);
                    cmd.Parameters.AddWithValue("@userId", post.UserId);
                    cmd.Parameters.AddWithValue("@whiskyId", post.WhiskyId);
                    cmd.Parameters.AddWithValue("@description", post.Description);
                    cmd.Parameters.AddWithValue("@rating", post.Rating);
                    cmd.Parameters.AddWithValue("@datetime", post.CreatedAt);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                throw new PostRepoException("An error occurred while creating the post.", ex);
            }
            catch (Exception ex)
            {
                throw new PostRepoException("An unexpected error occurred while creating the post.", ex);
            }
        }

        public List<PostDto> GetPosts()
        {
            try
            {
                List<PostDto> posts = new List<PostDto>();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM posts", conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PostDto post = new PostDto
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
            catch (MySqlException ex)
            {
                throw new PostRepoException("An error occurred while retrieving posts.", ex);
            }
            catch (Exception ex)
            {
                throw new PostRepoException("An unexpected error occurred while retrieving posts.", ex);
            }
        }

        public PostDto GetPostById(int id)
        {
            try
            {
                PostDto post = null;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM posts WHERE id = @id", conn);
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

                if (post == null)
                {
                    throw new NotFoundException($"Post with ID {id} was not found.");
                }

                return post;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (MySqlException ex)
            {
                throw new PostRepoException($"An error occurred while retrieving the post with ID {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new PostRepoException($"An unexpected error occurred while retrieving the post with ID {id}.", ex);
            }
        }

        public void UpdatePost(PostDto post)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("UPDATE posts SET user_id = @userId, whisky_id = @whiskyId, description = @description, rating = @rating, datetime = @datetime WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", post.Id);
                    cmd.Parameters.AddWithValue("@userId", post.UserId);
                    cmd.Parameters.AddWithValue("@whiskyId", post.WhiskyId);
                    cmd.Parameters.AddWithValue("@description", post.Description);
                    cmd.Parameters.AddWithValue("@rating", post.Rating);
                    cmd.Parameters.AddWithValue("@datetime", post.UpdatedAt);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        throw new NotFoundException($"Post with ID {post.Id} was not found for update.");
                    }
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (MySqlException ex)
            {
                throw new PostRepoException($"An error occurred while updating the post with ID {post.Id}.", ex);
            }
            catch (Exception ex)
            {
                throw new PostRepoException($"An unexpected error occurred while updating the post with ID {post.Id}.", ex);
            }
        }

        public void DeletePost(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("DELETE FROM posts WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        throw new NotFoundException($"Post with ID {id} was not found for deletion.");
                    }
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (MySqlException ex)
            {
                throw new PostRepoException($"An error occurred while deleting the post with ID {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new PostRepoException($"An unexpected error occurred while deleting the post with ID {id}.", ex);
            }
        }
    }
} 