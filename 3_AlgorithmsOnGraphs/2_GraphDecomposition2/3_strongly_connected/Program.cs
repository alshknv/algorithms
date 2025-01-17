﻿using System.Collections.Generic;
using System;
using System.Linq;

namespace _3_strongly_connected
{
    public class Vertex
    {
        public LinkedList<int> Destinations = new LinkedList<int>();
        public int CurrentDestination
        {
            get { return Destinations.First.Value; }
        }

        public LinkedList<int> DestinationsR = new LinkedList<int>();
        public int CurrentDestinationR
        {
            get { return DestinationsR.First.Value; }
        }

        public bool Visited;
        public int? PreVisit;
        public int? PostVisit;

        public void AddDestination(int destination)
        {
            Destinations.AddLast(destination);
        }
        public void AddDestinationR(int destination)
        {
            DestinationsR.AddLast(destination);
        }
    }
    public static class StronglyConnected
    {
        private static Vertex[] Vertices;

        private static int DFS(int index, int counter)
        {
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var vx = vertexStack.Peek();
                if (Vertices[vx].PreVisit == null) Vertices[vx].PreVisit = counter++;
                while (Vertices[vx].DestinationsR.Count > 0 &&
                        Vertices[Vertices[vx].CurrentDestinationR].PreVisit != null)
                {
                    Vertices[vx].DestinationsR.RemoveFirst();
                }
                if (Vertices[vx].DestinationsR.Count == 0)
                {
                    vertexStack.Pop();
                    Vertices[vx].PostVisit = counter++;
                }
                else
                {
                    vertexStack.Push(Vertices[vx].CurrentDestinationR);
                }
            }
            return counter;
        }

        private static void Explore(int index)
        {
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var vi = vertexStack.Peek();
                Vertices[vi].Visited = true;
                if (Vertices[vi].Destinations.Count == 0)
                {
                    vertexStack.Pop();
                }
                else
                {
                    while (Vertices[vi].Destinations.Count > 0 &&
                        Vertices[Vertices[vi].CurrentDestination].Visited)
                    {
                        Vertices[vi].Destinations.RemoveFirst();
                    }
                    if (Vertices[vi].Destinations.Count > 0)
                    {
                        vertexStack.Push(Vertices[vi].CurrentDestination);
                    }
                }
            }
        }

        public static string Solve(string[] input)
        {
            // graph init
            var graphInfo = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            Vertices = new Vertex[graphInfo[0] + 1];
            for (int i = 1; i <= graphInfo[0]; i++) Vertices[i] = new Vertex();
            for (int i = 0; i < graphInfo[1]; i++)
            {
                var edgeInfo = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                Vertices[edgeInfo[0]].AddDestination(edgeInfo[1]); // straight graph
                Vertices[edgeInfo[1]].AddDestinationR(edgeInfo[0]); // reversed graph
            }

            // DFS reversed graph to get postvisit indexes
            var v = 1;
            var counter = 0;
            while (v <= graphInfo[0])
            {
                while (v <= graphInfo[0] && Vertices[v].PreVisit != null) v++;
                if (v <= graphInfo[0])
                {
                    counter = DFS(v, counter);
                }
                v++;
            }

            // finding strongly connected components
            var postInfo = Vertices.Select((vrt, i) => new KeyValuePair<int, int?>(i, vrt?.PostVisit)).ToArray();
            Array.Sort(postInfo, (p1, p2) =>
            {
                if (p1.Value == null) return -1;
                if (p2.Value == null) return 1;
                return -((int)p1.Value).CompareTo((int)p2.Value);
            });

            v = 1;
            counter = 0;
            while (v <= graphInfo[0])
            {
                while (v <= graphInfo[0] && Vertices[postInfo[v].Key].Visited) v++;
                if (v <= graphInfo[0])
                {
                    Explore(postInfo[v].Key);
                    counter++;
                }
                v++;
            }
            return counter.ToString();
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
