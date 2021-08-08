using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace _5_organizing_a_lottery
{
    public static class OrganizingLottery
    {
        private class Segment
        {
            public long A;
            public long B;
            public Segment(long a, long b)
            {
                A = a;
                B = b;
            }
        }

        private enum PointType
        {
            SegmentBegin, BetPoint, SegmentEnd
        };

        private class Point
        {
            public PointType Type;
            public int Index;
            public long Coord;
            public Point(PointType type, long coord, int index = 0)
            {
                Type = type;
                Coord = coord;
                Index = index;
            }
        }

        private static Point[] Merge(Point[] arr1, Point[] arr2)
        {
            var mergedArray = new Point[arr1.Length + arr2.Length];
            var idx1 = 0;
            var idx2 = 0;
            while (idx1 < arr1.Length && idx2 < arr2.Length)
            {
                if (arr1[idx1].Coord < arr2[idx2].Coord)
                    mergedArray[idx1 + idx2] = arr1[idx1++];
                else if (arr1[idx1].Coord > arr2[idx2].Coord)
                    mergedArray[idx1 + idx2] = arr2[idx2++];
                else if (arr1[idx1].Type == PointType.SegmentBegin)
                    mergedArray[idx1 + idx2] = arr1[idx1++];
                else if (arr2[idx2].Type == PointType.SegmentBegin)
                    mergedArray[idx1 + idx2] = arr2[idx2++];
                else if (arr1[idx1].Type == PointType.BetPoint)
                    mergedArray[idx1 + idx2] = arr1[idx1++];
                else if (arr2[idx2].Type == PointType.BetPoint)
                    mergedArray[idx1 + idx2] = arr2[idx2++];
                else
                    mergedArray[idx1 + idx2] = arr1[idx1++];
            }
            if (idx1 < arr1.Length)
            {
                for (int i = idx1; i < arr1.Length; i++)
                {
                    mergedArray[i + idx2] = arr1[i];
                }
            }
            else if (idx2 < arr2.Length)
            {
                for (int i = idx2; i < arr2.Length; i++)
                {
                    mergedArray[idx1 + i] = arr2[i];
                }
            }
            return mergedArray;
        }

        private static Point[] SortPoints(Point[] points)
        {
            if (points.Length == 1)
            {
                return points;
            }

            var mid = points.Length / 2;
            return Merge(
                SortPoints(points.Take(mid).ToArray()),
                SortPoints(points.Skip(mid).ToArray())
            );
        }

        private static int[] CountSegments(long[] bets, Segment[] segments)
        {
            var points = new List<Point>();
            for (int i = 0; i < bets.Length; i++)
            {
                points.Add(new Point(PointType.BetPoint, bets[i], i));
            }

            points.AddRange(segments.Select(s => new Point(PointType.SegmentBegin, s.A)));
            points.AddRange(segments.Select(s => new Point(PointType.SegmentEnd, s.B)));
            var sortedPoints = SortPoints(points.ToArray());
            var nestCount = 0;

            var result = new int[bets.Length];
            for (int i = 0; i < sortedPoints.Length; i++)
            {
                switch (sortedPoints[i].Type)
                {
                    case PointType.SegmentBegin:
                        nestCount++;
                        break;
                    case PointType.SegmentEnd:
                        nestCount--;
                        break;
                    case PointType.BetPoint:
                        result[sortedPoints[i].Index] = nestCount;
                        break;
                }
            }
            return result;
        }

        public static string Solve(string pointLine, string[] segmentLines)
        {
            var betPoints = pointLine.Split(' ').Select(p => long.Parse(p)).ToArray();
            var segments = segmentLines.Select(s =>
            {
                var lineAb = s.Split(' ').Select(l => long.Parse(l)).ToArray();
                return new Segment(lineAb[0], lineAb[1]);
            }).ToArray();
            return string.Join(" ", CountSegments(betPoints, segments));
        }

        static void Main(string[] args)
        {
            var line1 = Console.ReadLine().Split(' ');
            var segmentCount = int.Parse(line1[0]);
            var segments = new string[segmentCount];
            for (var i = 0; i < int.Parse(line1[0]); i++)
            {
                segments[i] = Console.ReadLine();
            }
            var points = Console.ReadLine();
            Console.WriteLine(Solve(points, segments));
        }
    }
}
