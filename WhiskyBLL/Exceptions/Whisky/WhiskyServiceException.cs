using System;

namespace WhiskyBLL.Exceptions
{
    public class WhiskyServiceException : Exception
    {
        public WhiskyServiceException() : base("Whisky already exists.")
        {
        }

        public WhiskyServiceException(string message) : base(message)
        {
        }

        public WhiskyServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    public class WhiskyAlreadyExistsException : Exception
    {
        public WhiskyAlreadyExistsException() : base("Whisky already exists.")
        {
        }

        public WhiskyAlreadyExistsException(string message) : base(message)
        {
        }

        public WhiskyAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class InvalidWhiskyDataException : Exception
    {
        public InvalidWhiskyDataException() : base("Invalid whisky data.")
        {
        }

        public InvalidWhiskyDataException(string message) : base(message)
        {
        }

        public InvalidWhiskyDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}