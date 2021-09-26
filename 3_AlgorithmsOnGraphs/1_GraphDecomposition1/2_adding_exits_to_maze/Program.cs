using System.Collections.Generic;
using System;
using System.Linq;

namespace _2_adding_exits_to_maze
{
    public class Vertex
    {
        public LinkedList<int> Neighbours;
        public bool Visited;
        public Vertex()
        {
            Neighbours = new LinkedList<int>();
        }
    }

    //connected components problem
    public static class AddingExitsToMaze
    {
        private static Vertex[] Vertices;
        private static void Explore(int index)
        {
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var v = vertexStack.Peek();
                Vertices[v].Visited = true;
                while (Vertices[v].Neighbours.First != null && Vertices[Vertices[v].Neighbours.First.Value].Visited)
                {
                    Vertices[v].Neighbours.RemoveFirst();
                }
                if (Vertices[v].Neighbours.First != null)
                {
                    vertexStack.Push(Vertices[v].Neighbours.First.Value);
                }
                else
                {
                    vertexStack.Pop();
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
                Vertices[edgeInfo[0]].Neighbours.AddLast(edgeInfo[1]);
                Vertices[edgeInfo[1]].Neighbours.AddLast(edgeInfo[0]);
            }

            var cc = 0;
            var v = 1;
            while (v <= graphInfo[0])
            {
                while (v <= graphInfo[0] && Vertices[v].Visited) v++;
                if (v <= graphInfo[0])
                {
                    Explore(v);
                    cc++;
                }
            }
            return cc.ToString();
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
