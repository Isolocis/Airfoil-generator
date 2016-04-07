namespace NACAAirFoilGenerator
{
    public class AirfoilData
    {
        /// <summary>
        /// Gets or sets the numbers of the designation of the airfoil.
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Gets the designation including the NACA prefix.
        /// </summary>
        public string FullDesignation => $"NACA{this.Designation}";

        /// <summary>
        /// Gets or sets the number of nodes per side.
        /// </summary>
        public int NodesPerSide { get; set; }
    }
}