using System.Linq;
using System;

namespace _1_connecting_points
{
    public class Point
    {
        public int X;
        public int Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Edge
    {
        public int Point1;
        public int Point2;
        public double Distance;
    }

    public class DisjointTableItem
    {
        public int Index;
        public double Length;
        public DisjointTableItem(int index, double length)
        {
            Index = index;
            Length = length;
        }
    }

    public class DisjoinSet
    {
        private double maxLength = double.MinValue;
        private readonly DisjointTableItem[] items;
        public int Find(int i)
        {
            if (items[i].Length >= 0) return items[i].Index;
            var realTable = Find(items[i].Index);
            items[i].Index = items[realTable].Index;
            return realTable;
        }

        public DisjoinSet(int count)
        {
            var lengths = new double[count];
            items = lengths.Select((x, i) => new DisjointTableItem(i, x)).ToArray();
        }

        public void Union(int dst, int src, double length)
        {
            if (src != dst)
            {
                var source = Find(src);
                var destination = Find(dst);
                if (source != destination)
                {
                    items[destination].Length += items[source].Length + length;
                    if (items[destination].Length > maxLength) maxLength = items[destination].Length;
                    items[source].Index = items[destination].Index;
                    items[source].Length = -1;
                }
            }
        }

        public double MaxLength
        {
            get
            {
                return maxLength < 0 ? 0 : maxLength;
            }
        }
    }

    public static class BuildingRoads
    {
        private static double Kruskal(Edge[] edges, int vertexCount)
        {
            Array.Sort(edges, (e1, e2) => e1.Distance.CompareTo(e2.Distance));
            var dset = new DisjoinSet(vertexCount);
            for (int i = 0; i < edges.Length; i++)
            {
                if (dset.Find(edges[i].Point1) != dset.Find(edges[i].Point2))
                {
                    dset.Union(edges[i].Point1, edges[i].Point2, edges[i].Distance);
                }
            }
            return dset.MaxLength;
        }

        public static string Solve(string[] input)
        {
            var points = input.Select(x =>
            {
                var p = x.Split(' ').Select(y => int.Parse(y)).ToArray();
                return new Point(p[0], p[1]);
            }).ToArray();

            var edges = new Edge[points.Length * (points.Length - 1) / 2];
            var edgeCount = 0;
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    edges[edgeCount++] =
                        new Edge()
                        {
                            Point1 = i,
                            Point2 = j,
                            Distance = Math.Sqrt(Math.Pow(points[i].X - points[j].X, 2) + Math.Pow(points[i].Y - points[j].Y, 2))
                        };
                }
            }
            return Kruskal(edges, points.Length).ToString("0.000000000").Replace(",", ".");
        }

        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var input = new string[n];
            for (int i = 0; i < n; i++)
            {
                input[i] = Console.ReadLine();
            }
            Console.WriteLine(Solve(input));
        }
    }
}
