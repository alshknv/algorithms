using System.Collections.Generic;
using System;
using System.Linq;


namespace _2_toposort
{
    public class Vertex
    {
        public int[] Destinations;
        public int DestinationCount;
        public int DestinationIdx;
        public int CurrentDestination
        {
            get { return Destinations[DestinationIdx]; }
        }
        public bool Deleted;

        public void AddDestination(int destination)
        {
            Destinations[DestinationCount++] = destination;
        }

        public Vertex(int maxCount)
        {
            Destinations = new int[maxCount];
        }
    }

    public static class Toposort
    {
        private static Vertex[] Vertices;

        private static Stack<int> Sort;
        private static int SortIdx;

        private static void DFS(int index)
        {
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var v = vertexStack.Peek();
                if (Vertices[v].DestinationIdx >= Vertices[v].DestinationCount)
                {
                    // is sink
                    vertexStack.Pop();
                    Vertices[v].Deleted = true;
                    Sort.Push(v);
                }
                else
                {
                    while (Vertices[v].DestinationIdx < Vertices[v].DestinationCount &&
                        Vertices[Vertices[v].CurrentDestination].Deleted)
                    {
                        Vertices[v].DestinationIdx++;
                    }
                    if (Vertices[v].DestinationIdx < Vertices[v].DestinationCount)
                    {
                        vertexStack.Push(Vertices[v].Destinations[Vertices[v].DestinationIdx]);
                        Vertices[v].DestinationIdx++;
                    }
                }
            }
        }

        public static string Solve(string[] input)
        {
            // graph init
            var graphInfo = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            Sort = new Stack<int>();
            Vertices = new Vertex[graphInfo[0] + 1];
            for (int i = 1; i <= graphInfo[0]; i++) Vertices[i] = new Vertex(graphInfo[1]);
            for (int i = 0; i < graphInfo[1]; i++)
            {
                var edgeInfo = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                Vertices[edgeInfo[0]].AddDestination(edgeInfo[1]);
            }

            // topology sort
            var v = 1;
            while (v <= graphInfo[0])
            {
                while (v <= graphInfo[0] && Vertices[v].Deleted) v++;
                if (v <= graphInfo[0])
                {
                    DFS(v);
                }
                v++;
            }
            return string.Join(" ", Sort);
        }

        static void Main(string[] args)
        {
            var nmLine = Console.ReadLine();
            var edgeCount = int.Parse(nmLine.Split(' ').Last());
            var data = new string[edgeCount + 1];
            data[0] = nmLine;
            for (int i = 0; i < edgeCount; i++)
            {
                data[i + 1] = Console.ReadLine();
            }
            Console.WriteLine(Solve(data));
        }
    }
}
