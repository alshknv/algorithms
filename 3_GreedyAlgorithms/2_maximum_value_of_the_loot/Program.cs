using System.Collections.Generic;
using System.Linq;
using System;

namespace _2_maximum_value_of_the_loot
{
    public class MaximumValueOfTheLoot
    {
        private class ItemInfo
        {
            public double ValuePerWeight;
            public int Weight;
        }

        private static double GetMaxValue(int capacity, List<ItemInfo> items)
        {
            items.Sort((i1, i2) => i1.ValuePerWeight < i2.ValuePerWeight ? 1 : -1);
            int i = 0;
            double value = 0;
            while (i < items.Count && capacity > 0)
            {
                var item = items[i];
                int space2fill = Math.Min(item.Weight, capacity);
                value += item.ValuePerWeight * space2fill;
                capacity -= space2fill;
                i++;
            }
            return value;
        }

        public static string Solve(List<string> input)
        {
            int capacity = int.Parse(input[0].Split(' ')[1]);
            var items = input.Skip(1).Select(i =>
            {
                var itemInfo = i.Split(' ');
                var weight = int.Parse(itemInfo[1]);
                var value = (double)int.Parse(itemInfo[0]);
                return new ItemInfo()
                {
                    ValuePerWeight = value / weight,
                    Weight = weight
                };
            }).ToList();
            return GetMaxValue(capacity, items).ToString("0.0000").Replace(",", ".");
        }

        static void Main(string[] args)
        {
            var lines = new List<string>() {
                Console.ReadLine()
            };
            for (int i = 0; i < int.Parse(lines[0].Split(' ')[0]); i++)
            {
                lines.Add(Console.ReadLine());
            }
            Console.WriteLine(Solve(lines));
        }
    }
}
