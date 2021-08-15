using System;
namespace Temp
{
    class Program
    {
        static uint[] GetInput(string line1 = "", string line2 = "")
        {
            if (line1.Length == 0) line1 = Console.ReadLine();
            if (line2.Length == 0) line2 = Console.ReadLine();
            var size = int.Parse(line1);
            uint[] input = new uint[size];
            uint count = 0;
            foreach (var elm in line2.Split(' '))
            {
                input[count++] = uint.Parse(elm);
            }
            return input;
        }

        static void PrintAnswer(ulong answer)
        {
            Console.WriteLine(answer);
        }

        static uint[] GenerateData(int size, int limit)
        {
            var random = new Random();
            var result = new uint[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = (uint)random.Next(limit);
            }
            return result;
        }

        static ulong AlternateMaxProduction(uint[] numbers)
        {
            ulong product = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = i; j < numbers.Length; j++)
                {
                    ulong p = (ulong)numbers[i] * numbers[j];
                    if (i != j && p > product)
                    {
                        product = p;
                    }
                }
            }
            return product;
        }

        static ulong GetMaxProduction(uint[] numbers)
        {
            uint firstNum = 0;
            uint secondNum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > firstNum)
                {
                    secondNum = firstNum;
                    firstNum = numbers[i];
                }
                else if (numbers[i] > secondNum)
                {
                    secondNum = numbers[i];
                }
            }
            return (ulong)firstNum * secondNum;
        }

        static void Main(string[] args)
        {
            //var numbers = GetInput("5", "134466 558383 1399953 409928 1575864");
            // stress
            // while (true)
            // {
            //     var numbers = GenerateData(500, 2000000);
            //     if (GetMaxProduction(numbers) != AlternateMaxProduction(numbers))
            //     {
            //         Console.WriteLine(numbers.Length);
            //         Console.WriteLine(string.Join(" ", numbers));
            //         Console.WriteLine("Error");
            //         break;
            //     }
            //     else
            //     {
            //         Console.WriteLine("OK");
            //     }
            // }


            var numbers = GetInput();
            var result = GetMaxProduction(numbers);
            PrintAnswer(result);
            // result = AlternateMaxProduction(numbers);
            // PrintAnswer(result);
        }
    }
}
