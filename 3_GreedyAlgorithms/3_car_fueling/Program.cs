using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_car_fueling
{
    public class CarFueling
    {
        private static int CalculateRefills(int distance, int maxTravel, List<int> stops)
        {
            stops.Sort((s1, s2) => s1 > s2 ? 1 : -1);
            var refills = 0;
            var position = 0;
            while (position < distance)
            {
                if (position + maxTravel >= distance)
                {
                    return refills;
                }
                var stops2pass = stops.Where(s => s <= position + maxTravel).ToList();
                if (stops2pass.Count == 0)
                {
                    return -1;
                }
                position = stops2pass[stops2pass.Count - 1];
                stops = stops.Skip(stops2pass.Count).ToList();
                refills++;
            }
            if (position < distance)
            {
                return -1;
            }
            return refills;
        }
        public static string Solve(List<string> input)
        {
            var distance = int.Parse(input[0]);
            var maxTravel = int.Parse(input[1]);
            var stops = input[3].Split(' ').Select(s => int.Parse(s)).ToList();
            return CalculateRefills(distance, maxTravel, stops).ToString();
        }

        static void Main(string[] args)
        {
            var lines = new List<string>() {
                Console.ReadLine(),
                Console.ReadLine(),
                Console.ReadLine(),
                Console.ReadLine()
            };
            Console.WriteLine(Solve(lines));
        }
    }
}
