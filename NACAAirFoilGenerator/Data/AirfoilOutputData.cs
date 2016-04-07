namespace NACAAirFoilGenerator.Data
{
    /// <summary>
    /// Class containing generated airfoil data.
    /// </summary>
    public class AirfoilOutputData : AirfoilData
    {
        /// <summary>
        /// Gets or sets the x values for the lower nodes.
        /// </summary>
        public double[] XLower { get; set; }

        /// <summary>
        /// Gets or sets the x values for the upper nodes.
        /// </summary>
        public double[] XUpper { get; set; }

        /// <summary>
        /// Gets or sets the y values for the lower nodes.
        /// </summary>
        public double[] YLower { get; set; }

        /// <summary>
        /// Gets or sets the y values for the upper nodes.
        /// </summary>
        public double[] YUpper { get; set; }
        
    }
}