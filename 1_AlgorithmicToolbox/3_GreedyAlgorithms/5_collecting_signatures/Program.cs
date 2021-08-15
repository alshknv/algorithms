using System.Collections.Generic;
using System;
using System.Linq;

namespace _5_collecting_signatures
{
    public class CollectingSignatures
    {
        private class Segment
        {
            public int A;
            public int B;
        }

        private static int MinimumPoints(List<Segment> segments, out List<int> points)
        {
            segments.Sort((s1, s2) => s1.B.CompareTo(s2.B));
            points = new List<int>();
            while (segments.Count > 0)
            {
                var point = segments[0].B;
                segments.RemoveAll(s => s.A <= point && s.B >= point);
                points.Add(point);
            }
            points.Sort();
            return points.Count;
        }

        public static List<string> Solve(List<string> input)
        {
            var segments = input.Select(s =>
            {
                var coords = s.Split(' ');
                return new Segment() { A = int.Parse(coords[0]), B = int.Parse(coords[1]) };
            }).ToList();
            List<int> points;
            var result = new List<string>() {
                MinimumPoints(segments, out points).ToString()
            };
            result.Add(string.Join(" ", points));
            return result;
        }

        static void Main(string[] args)
        {
            var lines = new List<string>();
            var count = Console.ReadLine();
            for (int i = 0; i < int.Parse(count); i++)
            {
                lines.Add(Console.ReadLine());
            }
            foreach (var line in Solve(lines))
                Console.WriteLine(line);
        }
    }
}
