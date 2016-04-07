namespace NACAAirFoilGenerator
{
    /// <summary>
    /// Contains constants for generating a NACA four-digit airfoil.
    /// </summary>
    public static class NACAFourDigitConstants
    {
        /// <summary>
        /// The a0 constant.
        /// </summary>
        public const double a0 = 0.2969;

        /// <summary>
        /// The a1 constant.
        /// </summary>
        public const double a1 = -0.126;

        /// <summary>
        /// The a2 constant.
        /// </summary>
        public const double a2 = -0.3516;

        /// <summary>
        /// The a3 constant.
        /// </summary>
        public const double a3 = 0.2843;

        /// <summary>
        /// The a4 constant for an open trailing edge.
        /// </summary>
        public const double a4OpenTrailingEdge = -0.1015;

        /// <summary>
        /// The a4 constant for a closed trailing edge.
        /// </summary>
        public const double a4ClosedTrailingEdge = -0.1036;
    }
}