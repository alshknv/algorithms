using System.Collections.Generic;
using System;
using System.Linq;

namespace _1_acyclicity
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
        public bool Visited;

        public void AddDestination(int destination)
        {
            Destinations[DestinationCount++] = destination;
        }

        public Vertex(int maxCount)
        {
            Destinations = new int[maxCount];
        }
    }

    public static class Acyclicity
    {
        private static Vertex[] Vertices;

        private static bool ContainsCycle(int index)
        {
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var v = vertexStack.Peek();
                Vertices[v].Visited = true;
                if (Vertices[v].DestinationIdx >= Vertices[v].DestinationCount)
                {
                    // is sink
                    vertexStack.Pop();
                    Vertices[v].Deleted = true;
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
                        if (Vertices[Vertices[v].CurrentDestination].Visited)
                        {
                            return true;
                        }
                        else
                        {
                            vertexStack.Push(Vertices[v].Destinations[Vertices[v].DestinationIdx]);
                            Vertices[v].DestinationIdx++;
                        }
                    }
                }
            }
            return false;
        }

        public static string Solve(string[] input)
        {
            // graph init
            var graphInfo = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            Vertices = new Vertex[graphInfo[0] + 1];
            for (int i = 1; i <= graphInfo[0]; i++) Vertices[i] = new Vertex(graphInfo[1]);
            for (int i = 0; i < graphInfo[1]; i++)
            {
                var edgeInfo = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                Vertices[edgeInfo[0]].AddDestination(edgeInfo[1]);
            }

            // check if there's a cycle
            var v = 1;
            while (v <= graphInfo[0])
            {
                while (v <= graphInfo[0] && Vertices[v].Visited) v++;
                if (v <= graphInfo[0])
                {
                    if (ContainsCycle(v)) return "1";
                }
                v++;
            }
            return "0";
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
