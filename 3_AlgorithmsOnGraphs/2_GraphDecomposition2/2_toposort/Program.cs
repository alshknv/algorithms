using System.Collections.Generic;
using System;
using System.Linq;


namespace _2_toposort
{
    public class Vertex
    {
        public LinkedList<int> Destinations;
        public int CurrentDestination
        {
            get { return Destinations.First.Value; }
        }
        public bool Deleted;
        public Vertex()
        {
            Destinations = new LinkedList<int>();
        }
        public void AddDestination(int destination)
        {
            Destinations.AddLast(destination);
        }
    }

    public static class Toposort
    {
        private static void DFS(Vertex[] vertices, Stack<int> sort, int index)
        {
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var vx = vertexStack.Peek();
                while (vertices[vx].Destinations.Count > 0 && vertices[vertices[vx].CurrentDestination].Deleted)
                {
                    vertices[vx].Destinations.RemoveFirst();
                }
                if (vertices[vx].Destinations.Count == 0)
                {
                    // is sink
                    vertexStack.Pop();
                    vertices[vx].Deleted = true;
                    sort.Push(vx);
                }
                else
                {
                    vertexStack.Push(vertices[vx].CurrentDestination);
                }
            }
        }

        public static string Solve(string[] input)
        {
            // graph init
            var graphInfo = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var sort = new Stack<int>();
            var vertices = new Vertex[graphInfo[0] + 1];
            for (int k = 1; k <= graphInfo[0]; k++)
            {
                vertices[k] = new Vertex();
            }
            for (int i = 0; i < graphInfo[1]; i++)
            {
                var edgeInfo = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                vertices[edgeInfo[0]].AddDestination(edgeInfo[1]);
            }

            // topology sort
            var v = 1;
            while (v <= graphInfo[0])
            {
                while (v <= graphInfo[0] && vertices[v].Deleted) v++;
                if (v <= graphInfo[0])
                {
                    DFS(vertices, sort, v);
                    v++;
                }
            }
            return string.Join(" ", sort);
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
