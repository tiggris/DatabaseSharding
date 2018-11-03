using System;

namespace DatabaseSharding.Schools.Infrastructure.Data.Database
{
    public class InvalidConnectionStringException : Exception
    {
        public InvalidConnectionStringException(string message) : base(message)
        {
        }
    }
}
