using NACAAirFoilGenerator.Exceptions;

namespace NACAAirFoilGenerator.Data
{
    public class AirfoilInputData : AirfoilData
    {
        private double maxCamber;
        private double maxCamberPosition;
        private double thickness;

        /// <summary>
        /// Indicates if the trailing edge has to be closed.
        /// </summary>
        public bool CloseTrailingEdge { get; set; }
        
        /// <summary>
        /// Indicates if half cosine spacing should be used.
        /// </summary>
        public bool UseHalfCosineSpacing { get; set; }

        /// <summary>
        /// Gets the maximum camber.
        /// </summary>
        public double MaxCamber => this.maxCamber;

        /// <summary>
        /// Gets the maximum camber position.
        /// </summary>
        public double MaxCamberPosition => this.maxCamberPosition;

        /// <summary>
        /// Gets the thickness.
        /// </summary>
        public double Thickness => this.thickness;

        /// <summary>
        /// Checks if the given designation is valid.
        /// </summary>
        /// <returns>True if input is parseable</returns>
        public bool IsValid(out string error)
        {
            error = "";

            try
            {
                this.InterpretDesignation();
            }
            catch (InvalidAirfoilDataException ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts the input designation to a maximum camber, maximum camber position and thickness.
        /// </summary>
        private void InterpretDesignation()
        {
            if (!double.TryParse(this.Designation.Substring(0, 1), out this.maxCamber))
            {
                throw new InvalidAirfoilDataException($"Error parsing designation, could not convert the first digit: {this.Designation.Substring(0, 1)}");
            }
            this.maxCamber /= 100;

            if (!double.TryParse(this.Designation.Substring(1, 1), out this.maxCamberPosition))
            {
                throw new InvalidAirfoilDataException($"Error parsing designation, could not convert second digit: {this.Designation.Substring(1, 1)}");
            }
            this.maxCamberPosition /= 10;

            if (!double.TryParse(this.Designation.Substring(2, 2), out this.thickness))
            {
                throw new InvalidAirfoilDataException(
                    $"Error parsing designation, could not convert third and fourth digit: {this.Designation.Substring(2, 2)}");
            }
            this.thickness /= 100;

            if (this.maxCamber < 0 || this.maxCamber > 0.095)
                throw new InvalidAirfoilDataException($"Invalid maximum camber, values between 0% and 9.5% allowed. Input was: {this.maxCamber * 100}");

            if (this.maxCamberPosition < 0 || this.maxCamberPosition > 0.9)
                throw new InvalidAirfoilDataException($"Invalid maximum camber position, values between 0% and 90% allowed. Input was: {this.maxCamberPosition * 10}");

            if (this.thickness < 0.01 || this.thickness > 0.4)
                throw new InvalidAirfoilDataException($"Invalid thickness, values between 1 and 40% allowed. Input was: {this.thickness * 100}");
        }
    }
}