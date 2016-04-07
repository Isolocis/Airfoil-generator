using System;

namespace NACAAirFoilGenerator.Exceptions
{
    public class InvalidAirfoilDataException : Exception
    {
        public InvalidAirfoilDataException(string message) : base(message)
        {
        }

        public InvalidAirfoilDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}