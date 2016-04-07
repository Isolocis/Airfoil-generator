using System;

namespace NACAAirFoilGenerator.Exceptions
{
    public class SaveAirfoilDataException : Exception
    {
        public SaveAirfoilDataException()
        {
        }

        public SaveAirfoilDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}