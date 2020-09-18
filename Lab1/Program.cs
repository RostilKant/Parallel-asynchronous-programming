using System;

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
            var matrix = Calculator.CreateAndFillMatrix(10);
            PrintMatrix(matrix);

            Calculator.TransformMatrix(matrix);            
            Console.WriteLine();

            PrintMatrix(matrix);
        }
    }
}