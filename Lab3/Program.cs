using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3
{
    internal static class Program
    {
        private static int CountEvenArrayNumbers(IEnumerable<int> array)
        {
            int count = 0, countAtom = 0;

            Parallel.ForEach
                (
                    array,
                    element =>
                    {
                        int oldValue;
                        int newValue;

                        if (element % 2 == 0)
                        {
                            count++;
                        }
                        do
                        {
                            oldValue = countAtom;

                            if (element % 2 == 0)
                            {
                                newValue = oldValue + 1;
                            }
                            else
                            {
                                newValue = oldValue;
                            }
                        }
                        while (oldValue != Interlocked.CompareExchange(ref countAtom, newValue, oldValue));
                    }
                );

            Console.WriteLine($"-> Simple count: {count}  | Atomic count: {countAtom}");

            return countAtom;
        }

        private static int GetMinElementIndex(int[] array)
        {
            int minIndex = 0, minIndexAtom = 0;

            Parallel.For
                (
                    1,
                    array.Length,
                    index =>
                    {
                        int oldValue;
                        int newValue;

                        do
                        {
                            if (array[index].CompareTo(array[minIndex]) < 0)
                            {
                                minIndex = index;
                            }

                            oldValue = minIndexAtom;

                            newValue = array[index].CompareTo(array[minIndexAtom]) < 0 ? index : oldValue;
                        }
                        while (oldValue != Interlocked.CompareExchange(ref minIndexAtom, newValue, oldValue));
                    }
                );

            Console.WriteLine($"-> Common minimal index: {minIndex} | Atomic minimal index: {minIndexAtom}");

            return minIndex;
        }

        private static int GetMaxElementIndex(int[] array)
        {
            var maxArrayIndex = 0;
            var maxIndexAtom = 0;

            Parallel.For
                (
                    1,
                    array.Length,
                    index =>
                    {
                        int oldValue;
                        int newValue;

                        do
                        {
                            if (array[index].CompareTo(array[maxArrayIndex]) > 0)
                            {
                                maxArrayIndex = index;
                            }

                            oldValue = maxIndexAtom;

                            newValue = array[index].CompareTo(array[maxIndexAtom]) > 0 ? index : oldValue;
                        }
                        while (oldValue != Interlocked.CompareExchange(ref maxIndexAtom, newValue, oldValue));
                    }
                );

            Console.WriteLine($"-> Common max index: {maxArrayIndex} | Atomic max index: {maxIndexAtom}");

            return maxArrayIndex;
        }

        private static int XorCheckSum(IEnumerable<int> array)
        {
            var checkSum = 0;
            var checkSumAtom = 0;

            Parallel.ForEach
                (
                    array,
                    element =>
                    {
                        int oldValue;
                        int newValue;

                        checkSum ^= element;

                        do
                        {
                            oldValue = checkSumAtom;
                            newValue = oldValue ^ element;
                        }
                        while (oldValue != Interlocked.CompareExchange(ref checkSumAtom, newValue, oldValue));
                    }
                );

            Console.WriteLine($"-> Common checksum: {checkSum} | Atomic checksum: {checkSumAtom}");

            return checkSumAtom;
        }

        private static void Main(string[] args)
        {
            var array = new int[100500];

            var rand = new Random();

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = rand.Next(0, 1_000_000_000);
            }

            Console.WriteLine($"Count of even numbers: {CountEvenArrayNumbers(array)}\n");

            var min = GetMinElementIndex(array);
            Console.WriteLine($"Minimal element index: {min}, min element value: {array[min]}\n");

            var max = GetMaxElementIndex(array);
            Console.WriteLine($"Maximal element index: {max}, max element value: {array[max]}\n");

            Console.WriteLine($"Checksum: {XorCheckSum(array)}");
        }
    }
}
