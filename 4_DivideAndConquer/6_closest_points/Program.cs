using System.Collections.Generic;
using System;
using System.Linq;

namespace _6_closest_points
{
    public class Point
    {
        public long X;
        public long Y;

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }
    }

    public static class ClosestPoints
    {
        private static decimal SquareRoot(long x)
        {
            if (x == 0) return 0;
            decimal n = (decimal)Math.Sqrt(x);
            decimal lstX = 0.0m;
            while (n != lstX)// (n > lstX ? n - lstX : lstX - n) >= 0.0000000001m)
            {
                lstX = n;
                n = (n + x / n) / 2.0m;
            }
            return n;
        }

        private static decimal DecimalDistance(Point p1, Point p2)
        {
            return SquareRoot(((p1.X - p2.X) * (p1.X - p2.X)) + ((p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }

        private static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private enum ValueType { X, Y }

        private static int IndexSearch(Point[] points, ValueType type, int l, int r, double value)
        {
            if (r < l) return r;
            var mid = (int)Math.Floor((double)(r - l) / 2 + l);
            if ((type == ValueType.X ? points[mid].X : points[mid].Y) == value) return mid;
            if ((type == ValueType.X ? points[mid].X : points[mid].Y) >= value) return IndexSearch(points, type, l, mid - 1, value);
            else return IndexSearch(points, type, mid + 1, r, value);
        }

        private static decimal GetMinDistance(Point[] points)
        {
            if (points.Length < 2) return decimal.MaxValue;
            if (points.Length == 2)
            {
                return DecimalDistance(points[0], points[1]);
            }

            double midX = (double)(points[0].X + points[points.Length - 1].X) / 2;
            double? midY = null;

            var idx = IndexSearch(points, ValueType.X, 0, points.Length - 1, midX) + 1;
            var group1 = points.Take(idx).ToArray();
            var group2 = points.Skip(idx).ToArray();

            if (group2.Length == 0)
            {
                midY = (double)(points[0].Y + points[points.Length - 1].Y) / 2;
                idx = IndexSearch(points, ValueType.Y, 0, points.Length - 1, (double)midY) - 1;
                group1 = points.Take(idx).ToArray();
                group2 = points.Skip(idx).ToArray();
                if (group2.Length == 0)
                {
                    return 0;
                }
            }

            var minLeft = GetMinDistance(group1);
            var minRight = GetMinDistance(group2);
            var minDistance1 = Math.Min(minLeft, minRight);

            Point[] points2;

            if (midY == null)
            {
                var idx1 = IndexSearch(points, ValueType.X, 0, points.Length - 1, midX - (double)minDistance1);
                var idx2 = IndexSearch(points, ValueType.X, 0, points.Length - 1, midX + (double)minDistance1);
                points2 = points.Skip(idx1 + 1).Take(idx2 - idx1).ToArray();
            }
            else
            {
                var idx1 = IndexSearch(points, ValueType.Y, 0, points.Length - 1, (double)midY - (double)minDistance1);
                var idx2 = IndexSearch(points, ValueType.Y, 0, points.Length - 1, (double)midY + (double)minDistance1);
                points2 = points.Skip(idx1 + 1).Take(idx2 - idx1).ToArray();
            }

            var minDistance2 = decimal.MaxValue;
            for (int i = 0; i < points2.Length; i++)
            {
                for (int j = i + 1; j - i <= 7 && j < points2.Length; j++)
                {
                    var distance = DecimalDistance(points2[i], points2[j]);
                    if (distance < minDistance2)
                    {
                        minDistance2 = distance;
                    }
                }
            }
            return Math.Min(minDistance1, minDistance2);
        }

        public static string Naive(string[] input)
        {
            var points = input.Select(x =>
                {
                    var line = x.Split(' ');
                    return new Point(long.Parse(line[0]), long.Parse(line[1]));
                }).ToArray();
            var minDistance = decimal.MaxValue;
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    var distance = DecimalDistance(points[i], points[j]);
                    if (distance < minDistance)
                        minDistance = distance;
                }
            }
            return minDistance.ToString().Replace(",", ".");
        }

        public static string Solve(string[] input)
        {
            var points = input.Select(x =>
                {
                    var line = x.Split(' ');
                    return new Point(long.Parse(line[0]), long.Parse(line[1]));
                }).OrderBy(p => p.X).ThenBy(p => p.Y).ToArray();
            var minDistance = GetMinDistance(points);
            return minDistance.ToString().Replace(",", ".");
        }

        static void Main(string[] args)
        {
            var count = int.Parse(Console.ReadLine());
            var lines = new string[count];
            for (int i = 0; i < count; i++)
            {
                lines[i] = Console.ReadLine();
            }
            Console.WriteLine(Solve(lines));
        }
    }
}
