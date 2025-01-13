using System;

namespace WhiskyBLL.Exceptions.Post
{
  public class PostRepoException : Exception
  {
    public PostRepoException() : base("An error occurred in the Post Repository.")
    {
    }

    public PostRepoException(string message) : base(message)
    {
    }

    public PostRepoException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}