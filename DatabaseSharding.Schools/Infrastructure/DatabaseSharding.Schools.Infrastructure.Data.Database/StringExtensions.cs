using System.Security;

namespace DatabaseSharding.Schools.Infrastructure.Data.Database
{
    public static class StringExtensions
    {
        public static SecureString ToSecureString(this string input)
        {
            var secureString = new SecureString();
            foreach(var c in input.ToCharArray())
            {
                secureString.AppendChar(c);
            }            
            return secureString;
        }
    }
}
