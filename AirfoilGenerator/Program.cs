using System;
using NACAAirFoilGenerator;
using NACAAirFoilGenerator.Data;

namespace AirfoilGeneratorConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputData = new AirfoilInputData();

            Console.Write("4 digit NACA identifier: ");
            inputData.Designation = Console.ReadLine();

            Console.Write("Number of nodes per side: ");
            inputData.NodesPerSide = int.Parse(Console.ReadLine());

            Console.Write("Close trailing edge (y/n): ");
            inputData.CloseTrailingEdge = Console.ReadKey().Key == ConsoleKey.Y;
            Console.Write(Environment.NewLine);

            Console.Write("Use half cosine spacing (y/n): ");
            inputData.UseHalfCosineSpacing = Console.ReadKey().Key == ConsoleKey.Y;
            Console.Write(Environment.NewLine);

            Console.Write("Include 3rd coordinate (y/n): ");
            var includeThirdCoordinate = Console.ReadKey().Key == ConsoleKey.Y;
            Console.Write(Environment.NewLine);

            Console.Write("Output directory: ");
            var outputFileDirectory = Console.ReadLine();

            var outputPath = AirfoilGenerator.GenerateAirfoilData(inputData);
            Console.WriteLine($"Results written to {outputPath}");
            Console.Read();
        }
    }
}
