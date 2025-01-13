using System;

namespace WhiskyBLL.Exceptions.Whisky
{
  public class WhiskyRepoException : Exception
  {
    public WhiskyRepoException() : base("An error occurred in the Whisky Repository.")
    {
    }

    public WhiskyRepoException(string message) : base(message)
    {
    }

    public WhiskyRepoException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}