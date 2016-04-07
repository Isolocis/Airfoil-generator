using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NACAAirFoilGenerator.Data;
using NACAAirFoilGenerator.Exceptions;

namespace NACAAirFoilGenerator
{
    public static class AirfoilGenerator
    {
        private static double a4, maxCamber, maxCamberPosition, thickness;
        private static double[] x;
        private static bool closeTrailingEdge;

        /// <summary>
        /// Generates airfoil data from the given input data.
        /// </summary>
        /// <param name="inputData">The data to process.</param>
        /// <returns>Generated coordinates for the given airfoil specification.</returns>
        public static AirfoilOutputData GenerateAirfoilData(AirfoilInputData inputData)
        {
            // Read input data.
            string error;
            if(!inputData.IsValid(out error))
            {
                throw new InvalidAirfoilDataException(error);
            }

            if (inputData.Designation.Length == 4)
                return GenerateFourDigitAirfoilData(inputData);

            throw new InvalidAirfoilDataException("Only 4 digit airfoils are supported at this time.");
        }

        /// <summary>
        /// Generates NACA 4 digit airfoil data from the given input data.
        /// </summary>
        /// <param name="inputData">The data to process.</param>
        /// <returns>Generated coordinates for the given airfoil specification.</returns>
        public static AirfoilOutputData GenerateFourDigitAirfoilData(AirfoilInputData inputData)
        {
            maxCamber = inputData.MaxCamber;
            maxCamberPosition = inputData.MaxCamberPosition;
            thickness = inputData.Thickness;
            closeTrailingEdge = inputData.CloseTrailingEdge;

            a4 = inputData.CloseTrailingEdge ? NACAFourDigitConstants.a4ClosedTrailingEdge : NACAFourDigitConstants.a4OpenTrailingEdge;

            // Generate the x coordinates
            GenerateX(inputData.UseHalfCosineSpacing, inputData.NodesPerSide);

            // Get the thickness distribution
            var yThickness =
                x.Select(
                    xx =>
                        thickness / 0.2 *
                        (NACAFourDigitConstants.a0 * Math.Sqrt(xx) + NACAFourDigitConstants.a1 * xx +
                         NACAFourDigitConstants.a2 * xx * xx + NACAFourDigitConstants.a3 * xx * xx * xx +
                         a4 * xx * xx * xx * xx)).ToList();

            if (maxCamberPosition == 0)
            {
                return CreateSymmetricAirfoil(yThickness, inputData.Designation);
            }

            var xFront = x.Where(xx => xx <= maxCamberPosition).ToList();
            var xBack = x.Where(xx => xx > maxCamberPosition).ToList();
            return CreateAsymmetricAirfoil(xFront, xBack, yThickness, inputData.Designation);
        }



        /// <summary>
        /// Generates an array of x-coordinates
        /// </summary>
        /// <param name="halfCosineSpacing">Indicates if half cosine spacing should be used.</param>
        /// <param name="numberOfNodes">The number of nodes.</param>
        private static void GenerateX(bool halfCosineSpacing, int numberOfNodes)
        {
            x = new double[numberOfNodes];
            if (halfCosineSpacing)
            {
                for (var i = 0; i < numberOfNodes; i++)
                {
                    x[i] = 0.5 * (1 - Math.Cos(i * Math.PI / (numberOfNodes - 1)));
                }
            }
            else
            {
                for (var i = 0; i < numberOfNodes; i++)
                {
                    x[i] = (double)i / (numberOfNodes - 1);
                }
            }

            x[numberOfNodes - 1] = 1;
        }

        /// <summary>
        /// Creates a symmetrical airfoil.
        /// </summary>
        /// <param name="yThickness">The thickness distribution.</param>        
        /// <param name="designation">The designation of the generated airfoil.</param>
        /// <returns>The generated airfoil coordinates</returns>
        private static AirfoilOutputData CreateSymmetricAirfoil(List<double> yThickness, string designation)
        {
            var result = new AirfoilOutputData
            {
                NodesPerSide = yThickness.Count,
                Designation = designation,
                XUpper = x,
                YUpper = yThickness.ToArray(),
                XLower = x,
                YLower = yThickness.Select(y => -y).ToArray()
            };


            // Fix endpoints to the x-axis.
            result.XUpper[0] = 0;
            result.YLower[0] = 0;
            if (!closeTrailingEdge) return result;

            result.YUpper[result.YUpper.Length - 1] = 0;
            result.YLower[result.YLower.Length - 1] = 0;

            return result;
        }

        /// <summary>
        /// Creates an asymmetrical airfoil and saves it in <see cref="xUpper"/>, <see cref="yUpper"/>, <see cref="xLower"/> and <see cref="yLower"/>
        /// </summary>
        /// <param name="xFront">The x coordinates before the maximum camber point.</param>
        /// <param name="xBack">The x coordinates after the maximum camber point.</param>
        /// <param name="yThickness">The thickness distribution.</param>
        /// <param name="designation">The airfoil designation.</param>
        /// <returns>The generated airfoil coordinates</returns>
        private static AirfoilOutputData CreateAsymmetricAirfoil(List<double> xFront, List<double> xBack, List<double> yThickness, string designation)
        {
            var yCamber =
                xFront.Select(
                    d =>
                        maxCamber / (maxCamberPosition * maxCamberPosition) * (2 * maxCamberPosition * d - d * d))
                    .ToList();
            yCamber.AddRange(xBack.Select(
                d =>
                    maxCamber / ((1 - maxCamberPosition) * (1 - maxCamberPosition)) *
                    (1 - 2 * maxCamberPosition + 2 * maxCamberPosition * d - d * d)));

            var dYCamberDx =
                xFront.Select(d => 2 * maxCamber / (maxCamberPosition * maxCamberPosition) * (maxCamberPosition - d))
                    .ToList();
            dYCamberDx.AddRange(
                xBack.Select(
                    d => 2 * maxCamber / ((1 - maxCamberPosition) * (1 - maxCamberPosition)) * (maxCamberPosition - d)));

            var theta = dYCamberDx.Select(Math.Atan).ToList();

            var xUpper = new double[x.Length];
            var yUpper = new double[x.Length];
            var xLower = new double[x.Length];
            var yLower = new double[x.Length];
            for (var i = 0; i < x.Length; i++)
            {
                var sinTheta = Math.Sin(theta[i]);
                var cosTheta = Math.Cos(theta[i]);

                xUpper[i] = x[i] - yThickness[i] * sinTheta;
                yUpper[i] = yCamber[i] + yThickness[i] * cosTheta;

                xLower[i] = x[i] + yThickness[i] * sinTheta;
                yLower[i] = yCamber[i] - yThickness[i] * cosTheta;
            }

            // Fix endpoints to the x-axis.
            yUpper[0] = 0;
            yLower[0] = 0;

            if (closeTrailingEdge)
            {
                yUpper[yUpper.Length - 1] = 0;
                yLower[yLower.Length - 1] = 0;
            }

            return new AirfoilOutputData
            {
                NodesPerSide = yThickness.Count,
                Designation = designation,
                XLower = xLower,
                XUpper = xUpper,
                YLower = yLower,
                YUpper = yUpper
            };
        }

        /// <summary>
        /// Writes the generated data to a file.
        /// </summary>
        /// <param name="data">The data to write.</param>
        /// <param name="filePath">The path of the output file.</param>
        /// <param name="includeThirdCoordinate">Indicates if a third coordinate has to be included (equal to zero everywhere)</param>
        public static void WriteOutputFile(AirfoilOutputData data, string filePath, bool includeThirdCoordinate)
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine($"{x.Length} 2");

                for (int i = 0; i < data.NodesPerSide; i++)
                {
                    writer.Write($" {data.XUpper[i]} {data.YUpper[i]}");

                    if(includeThirdCoordinate)
                        writer.Write(" 0");

                    writer.Write(Environment.NewLine);
                }

                for (int i = 0; i < data.NodesPerSide; i++)
                {
                    writer.Write($" {data.XLower[i]} {data.YLower[i]}");

                    if (includeThirdCoordinate)
                        writer.Write(" 0");

                    writer.Write(Environment.NewLine);
                }
            }
        }
    }
}
