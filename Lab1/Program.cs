using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lab1
{
    static class Program
    {
        private static void PrintMatrix(int [,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[j, i],3}");
                }
                Console.WriteLine();
            }            
        }

        private static void Main(string[] args)
        {
            var stopwatch1 = new Stopwatch();
            var stopwatch2 = new Stopwatch();
            var matrix1 = Calculator.CreateAndFillMatrix(4);
            var matrix2 = Calculator.CreateAndFillMatrix(2_0);

            PrintMatrix(matrix1);
            Console.WriteLine();
            
            Calculator.TransformMatrix(matrix1);
            PrintMatrix(matrix1);

            
        }
    }
}