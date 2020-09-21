using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lab1
{
    static class Program
    {
        private static void Main(string[] args)
        {
            
            var calc = new MatrixCalculator(10000,1200);
            var parallelTime = calc.CreateTransformPrintInfoParallel();
            
            var calc1 = new MatrixCalculator(10000,1);
            var syncTime = calc1.CreateTransformPrintInfo();
            var acceleration = syncTime / parallelTime;
            Console.WriteLine("Коефiцiєнт прискорення = " + acceleration);
            Console.WriteLine("Коефiцiєнт ефективностi = " + acceleration / 1200);
        }
    }
}