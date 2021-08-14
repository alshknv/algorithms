using System.Collections.Generic;
using System;

namespace _2_primitive_calculator
{
    public static class PrimitiveCalculator
    {
        public enum OperationType { Plus1, MultBy2, MultBy3, None };

        private static int GetPreviousNum(int currentNum, OperationType type)
        {
            switch (type)
            {
                case OperationType.Plus1:
                    return currentNum - 1;
                case OperationType.MultBy2:
                    return currentNum % 2 == 0 ? currentNum / 2 : -1;
                case OperationType.MultBy3:
                    return currentNum % 3 == 0 ? currentNum / 3 : -1;
                default:
                    return currentNum;
            }
        }

        public static string Naive(string input)
        {
            return "";
        }

        public static IEnumerable<string> Solve(string input)
        {
            var number = int.Parse(input);
            var numChain = new string[number + 1];
            var operChain = new int[number + 1];
            numChain[1] = "1";

            for (int n = 2; n <= number; n++)
            {
                var minOpers = int.MaxValue;
                var curNumber = "";
                for (var oper = OperationType.Plus1; oper <= OperationType.MultBy3; oper++)
                {
                    var prev = GetPreviousNum(n, oper);

                    if (prev >= 0 && operChain[prev] + 1 < minOpers)
                    {
                        minOpers = operChain[prev] + 1;
                        curNumber = $"{numChain[prev]} {n}";
                    }
                }
                operChain[n] = minOpers;
                numChain[n] = curNumber;
            }
            yield return operChain[number].ToString();
            yield return numChain[number];
        }

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            foreach (var line in Solve(input))
                Console.WriteLine(line);
        }
    }
}
