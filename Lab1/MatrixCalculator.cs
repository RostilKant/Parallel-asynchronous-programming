using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lab1
{
    public class MatrixCalculator
    {
        private int[,] Matrix { get; set; }

        private readonly Random _rand;
        private int _tasksNumber;

        private int TasksNumber 
        { 
            get => _tasksNumber;

            set
            {
                if (value > Matrix.Length / 2)
                {
                    _tasksNumber = Matrix.Length / 2;
                    return;
                }

                if (value <= 0)
                {
                    _tasksNumber = 1;
                    return;
                }

                _tasksNumber = value;
            }
        }
        
        private void PrintMatrix()
        {
            for (var i = 0; i < Matrix.GetLength(0); i++)
            {
                for (var j = 0; j < Matrix.GetLength(1); j++)
                {
                    Console.Write($"{Matrix[j, i],4}");
                }
                Console.WriteLine();
            }            
        }

        public void FillMatrix()
        {
            var n = Matrix.GetLength(0);
            var random = new Random();

            for (var x = 0; x < n; x++)
            {
                for (var y = 0; y < n; y++)
                {
                    Matrix[x, y] = random.Next(10);
                }
            }
        }

        public void TransformMatrix()
        {
            var size = Matrix.GetLength(0);
            for (var i = 0; i < size; i++)
            {
                var main = 0;
                for (var j = 0; j < size; j++)
                {
                    if (i != j)
                    {
                        main += Matrix[i, j];
                        main += Matrix[j, i];
                    }
                }
                Matrix[i, i] = main;
            }
           
        }

        private struct RangeAction
        {
            public int Start;
            public int End;
            public Action<int, int> elementAction;
        }

      
        public MatrixCalculator(int matrixSize, int tasksNumber)
        {
            _rand = new Random();
            _tasksNumber = tasksNumber;
            Matrix = new int[matrixSize,matrixSize];
        }
        

        private void RandomMatrix(int rowNumber, int columnNumber)
        {
            Matrix[rowNumber, columnNumber] = _rand.Next(10);
        }

        private void TransformMatrixPart(int rowNumber, int columnNumber)
        {
            if (rowNumber < Matrix.GetLength(0))
            {
                if (rowNumber != columnNumber)
                {
                    Matrix[rowNumber, rowNumber] += Matrix[rowNumber, columnNumber];
                    Matrix[rowNumber, rowNumber] += Matrix[columnNumber, rowNumber];
                }
            }
        }

        private void TransformDiagonalToZero(int rowNumber, int columnNumber)
        {
            if (rowNumber == columnNumber)
                Matrix[rowNumber, columnNumber] = 0;
        }

        private Task MatrixElementsRange(object elemRangeActionObj)
        {
            var matrixSize = Matrix.GetLength(0);
            return Task.Run(() =>
            {
                if (elemRangeActionObj is RangeAction elemRangeAction)
                {
                    if (elemRangeAction.elementAction == null)
                        return;

                    int rowNumber = elemRangeAction.Start / matrixSize;
                    int columnNumber = elemRangeAction.Start % matrixSize;

                    for (int i = elemRangeAction.Start; i < elemRangeAction.End; i++)
                    {
                        elemRangeAction.elementAction(rowNumber, columnNumber);
                        columnNumber++;
                        if (columnNumber == matrixSize)
                        {
                            columnNumber = 0;
                            rowNumber++;
                        }
                    }
                }
            });
        }

        private void DoMatrixTaskByParts(int startInd, int endInd, Action<int, int> action)
        {
            var wholeRange = endInd - startInd;
            var basePortionRangeSize = wholeRange / TasksNumber;
            var remainder = wholeRange % TasksNumber;
            var currentStartIndex = startInd;

            var tasks = new Task[TasksNumber];

            for (var i = 0; i < TasksNumber; i++)
            {
                var currentRowsRangeSize = basePortionRangeSize;
                if (i < remainder)
                {
                    currentRowsRangeSize++;
                }

                tasks[i] = MatrixElementsRange(new RangeAction
                {
                    Start = currentStartIndex,
                    End = (currentStartIndex += currentRowsRangeSize),
                    elementAction = action
                });
            }

            Task.WaitAll(tasks);
        }

        public void FillMatrixParallel()
        {
            DoMatrixTaskByParts(0, Matrix.Length, RandomMatrix);
        }

        public void DiagonalToZeroParallel()
        {
            DoMatrixTaskByParts(0, Matrix.Length, TransformDiagonalToZero);
        }

        
        public void TransformMatrixParallel()
        {
            DoMatrixTaskByParts(0, Matrix.Length, TransformMatrixPart);
        }

        public double CreateTransformPrintInfoParallel()
        {
            var sw = new Stopwatch();
            sw.Start();

            FillMatrixParallel();

            sw.Stop();

            var printParallel = sw.Elapsed.TotalMilliseconds;

            //PrintMatrix();
            Console.WriteLine();
            
            Console.WriteLine($"Matrix with size {Matrix.GetLength(0)} was created and filled " +
                              $"by {TasksNumber} threads: {sw.Elapsed.TotalMilliseconds}");
            Console.WriteLine();

            sw.Restart();

            DiagonalToZeroParallel();
            TransformMatrixParallel();
            
            sw.Stop();
            var transformParallel = sw.Elapsed.TotalMilliseconds;
            
            //PrintMatrix();
            Console.WriteLine();
            Console.WriteLine($"Matrix of size {Matrix.GetLength(0)} calculated sum of elements " +
                              $"in the same row & column and replace with main diagonal elements  " +
                              $"with {TasksNumber} threads: {sw.Elapsed.TotalMilliseconds}");

            return printParallel + transformParallel;
        }
        
        public double CreateTransformPrintInfo()
        {
            var sw = new Stopwatch();
            sw.Start();

            FillMatrix();

            sw.Stop();
            var print = sw.Elapsed.TotalMilliseconds;

            Console.WriteLine("*************************SYNCHRONIZER************************");
            Console.WriteLine();

            //PrintMatrix();

            Console.WriteLine($"Matrix with size {Matrix.GetLength(0)} was created and filled " +
                              $"by {TasksNumber} threads: {sw.Elapsed.TotalMilliseconds}");
            Console.WriteLine();

            sw.Restart();
            
            TransformMatrix();
            
            sw.Stop();
            var transform = sw.Elapsed.TotalMilliseconds;

            //PrintMatrix();
            Console.WriteLine();
            Console.WriteLine($"Matrix of size {Matrix.GetLength(0)} calculated sum of elements " +
                              $"in the same row & column and replace with main diagonal elements  " +
                              $": {sw.Elapsed.TotalMilliseconds}");
            
            Console.WriteLine();

            return print + transform;
        }
    }
}