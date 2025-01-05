using System;

namespace WhiskyBLL.Exceptions
{
    public class PostServiceException : Exception
    {
        public PostServiceException() : base("An error occurred in the Post Service.")
        {
        }

        public PostServiceException(string message) : base(message)
        {
        }

        public PostServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class InvalidPostDataException : Exception
    {
        public InvalidPostDataException() : base("Invalid post data.")
        {
        }

        public InvalidPostDataException(string message) : base(message)
        {
        }

        public InvalidPostDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
} 