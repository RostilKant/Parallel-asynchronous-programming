using System;
using System.Threading.Tasks;

namespace Lab1
{
    public static class Calculator
    {
        public static int[,] CreateAndFillMatrix(int n)
        {
            var matrix = new int[n,n];
            var random = new Random();

            for (var x = 0; x < n; ++x)
            {
                for (var y = 0; y < n; ++y)
                {
                    matrix[x, y] = random.Next(100);
                }
            }
            return matrix;
        }

        public static void TransformMatrix(int[,] matrix)
        {
            var size = matrix.GetLength(0);
            
            for (var y = 0; y < size; y++)
            {
                var count = (y <= size/2) ? y : size/2 - (y - size/2);
                for (var x = 0; x <= count; x++)
                {
                    var temp = matrix[x, y];
                    matrix[x, y] = matrix[size - x - 1, y];
                    matrix[size - x - 1, y] = temp;
                }
            }
        }
    }
}