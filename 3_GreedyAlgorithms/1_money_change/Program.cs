using System;

namespace _1_money_change
{
    public class MoneyChange
    {
        private static readonly int[] coinValues = new int[3] { 10, 5, 1 };
        private static int MinimumCoins(int sum, int valueIdx)
        {
            int coins = sum / coinValues[valueIdx];
            if (valueIdx == coinValues.Length - 1)
                return coins;
            return coins + MinimumCoins(sum % coinValues[valueIdx], ++valueIdx);
        }

        public static string Solve(string input)
        {
            return MinimumCoins(int.Parse(input), 0).ToString();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
