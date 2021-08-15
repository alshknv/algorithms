using System;

namespace _1_money_change_again
{
    public static class MoneyChangeAgain
    {
        private readonly static int[] denominations = new int[] { 1, 3, 4 };

        public static string Naive(string input)
        {
            var money = int.Parse(input);
            var minCoins = int.MaxValue;
            for (int i = 0; i <= money; i++)
            {
                for (int j = 0; j <= money; j++)
                {
                    for (int k = 0; k <= money; k++)
                    {
                        var sum = denominations[0] * i + denominations[1] * j + denominations[2] * k;
                        var coins = i + j + k;
                        if (sum == money && coins > 0 && coins < minCoins)
                        {
                            minCoins = coins;
                        }
                    }
                }
            }
            return minCoins.ToString();
        }

        public static string Solve(string input)
        {
            var money = int.Parse(input);
            var coins = new int[money + 1];
            for (int m = 1; m < coins.Length; m++)
            {
                var minCoins = int.MaxValue;
                for (int j = 0; j < denominations.Length; j++)
                {
                    var idx = m - denominations[j];
                    if (idx < 0) break;
                    if (coins[idx] + 1 < minCoins)
                    {
                        minCoins = coins[idx] + 1;
                    }
                }
                coins[m] = minCoins;
            }
            return coins[money].ToString();
        }

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            Console.WriteLine(Solve(input));
        }
    }
}
