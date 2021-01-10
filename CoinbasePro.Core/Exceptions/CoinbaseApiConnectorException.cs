using System;

namespace CoinbasePro.Core.Exceptions
{
    public class CoinbaseApiConnectorException : Exception
    {
        public CoinbaseApiConnectorException(string message) : base(message)
        {
            
        }
        public CoinbaseApiConnectorException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}