using System;
using System.Threading.Tasks;

namespace Lab1
{
    public static class Calculator
    {
        public static int[,] CreateAndFillMatrix(int n)
        {
            var matrix = new int[n, n];
            var random = new Random();

            for (var x = 0; x < n; ++x)
            {
                for (var y = 0; y < n; ++y)
                {
                    matrix[x, y] = random.Next(5);
                }
            }

            return matrix;
        }

        public static void TransformMatrix(int[,] matrix)
        {
            var size = matrix.GetLength(0);
            for (var x = 0; x < size; ++x)
            {
                var main = 0;
                for (var y = 0; y < size; ++y)
                {
                    if (x != y)
                    {
                        main += matrix[x, y];
                        main += matrix[y, x];
                    }
                }

                matrix[x, x] = main;
            }
           
        }
    }
}