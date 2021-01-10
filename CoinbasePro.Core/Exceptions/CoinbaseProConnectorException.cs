using System;

namespace CoinbasePro.Core.Exceptions
{
    public class CoinbaseProConnectorException : Exception
    {
        public CoinbaseProConnectorException(string message) : base(message)
        {
            
        }
        public CoinbaseProConnectorException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}