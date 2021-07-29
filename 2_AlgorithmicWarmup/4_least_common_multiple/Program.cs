using System;

namespace _4_least_common_multiple
{
    public class LCM
    {
        private static uint GetGCD(uint a, uint b)
        {
            uint mod;
            do
            {
                mod = a % b;
                a = b;
                b = mod;
            } while (b > 0);
            return a;
        }

        private static ulong GetLCM(uint a, uint b)
        {
            return (ulong)a * b / GetGCD(a, b);
        }

        public static string Calc(string input)
        {
            var numbers = input.Split(' ');
            return GetLCM(uint.Parse(numbers[0]), uint.Parse(numbers[1])).ToString();
        }

        public static void Main(string[] args)
        {
            var input = Console.ReadLine();
            Console.WriteLine(Calc(input));
        }
    }
}
