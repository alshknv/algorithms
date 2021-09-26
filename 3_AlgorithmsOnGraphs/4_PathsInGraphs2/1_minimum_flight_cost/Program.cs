using System;
using System.Collections.Generic;
using System.Linq;

namespace _1_minimum_flight_cost
{
    public class Edge
    {
        public int Destination;
        public int Weight;
    }

    public class Vertex
    {
        public LinkedList<Edge> Edges;
        public int Distance = int.MaxValue;
        public bool Processed;
        public Vertex()
        {
            Edges = new LinkedList<Edge>();
        }
        public Vertex(Vertex vertex)
        {
            Edges = new LinkedList<Edge>(vertex.Edges);
            Distance = vertex.Distance;
        }
        public void AddEdge(int destination, int weight)
        {
            Edges.AddLast(new Edge() { Destination = destination, Weight = weight });
        }
    }

    public class VertexQueue
    {
        private List<Vertex> data;

        private void Swap(int a, int b)
        {
            var buf = data[a];
            data[a] = data[b];
            data[b] = buf;
        }

        private void SiftUp(int index)
        {
            var nextI = index;
            do
            {
                index = nextI;
                var p = (index - 1) / 2;
                if (data[p].Processed || (p >= 0 && (data[index].Distance < data[p].Distance)))
                {
                    nextI = p;
                    Swap(index, nextI);
                }
            } while (nextI != index);
        }

        private void SiftDown(int i)
        {
            while (i < data.Count)
            {
                var c1 = 2 * i + 1;
                var c2 = 2 * i + 2;
                var nextI = c1 < data.Count && data[c1].Distance < data[i].Distance ? c1 : i;
                nextI = c2 < data.Count && data[c2].Distance < data[nextI].Distance ? c2 : nextI;
                if (nextI != i)
                {
                    Swap(i, nextI);
                    i = nextI;
                }
                else
                {
                    break;
                }
            }
        }

        public VertexQueue(Vertex[] vertices)
        {
            data = new List<Vertex>(vertices);
            for (int i = (data.Count - 1) / 2; i > 0; i--)
            {
                SiftDown(i);
            }
        }

        public Vertex ExtractMin()
        {
            var result = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            SiftDown(0);
            return result;
        }

        public void ChangePriority(Vertex v)
        {
            data.Add(new Vertex(v));
            SiftUp(data.Count - 1);
        }

        public bool Empty()
        {
            return data.Count == 0;
        }
    }

    public static class Dijkstra
    {
        public static int GetDijkstraDistance(Vertex[] vertices, int start, int end)
        {
            vertices[start].Distance = 0;
            var queue = new VertexQueue(vertices.Skip(1).ToArray());

            while (!queue.Empty())
            {
                var u = queue.ExtractMin();
                if (u.Processed || u.Distance == int.MaxValue)
                {
                    continue;
                }
                while (u.Edges.First != null)
                {
                    var destVertex = vertices[u.Edges.First.Value.Destination];
                    if (destVertex.Distance > u.Distance + u.Edges.First.Value.Weight)
                    {
                        destVertex.Distance = u.Distance + u.Edges.First.Value.Weight;
                        destVertex.Processed = true;
                        queue.ChangePriority(destVertex);
                    }
                    u.Edges.RemoveFirst();
                }
            }
            return vertices[end].Distance;
        }

        public static string Solve(string[] input)
        {
            // graph init
            var graphInfo = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var vertices = new Vertex[graphInfo[0] + 1];
            for (int k = 1; k <= graphInfo[0]; k++)
            {
                vertices[k] = new Vertex();
            }
            for (int i = 0; i < graphInfo[1]; i++)
            {
                var edgeInfo = input[i + 1].Split(' ').Select(x => int.Parse(x)).ToArray();
                vertices[edgeInfo[0]].AddEdge(edgeInfo[1], edgeInfo[2]);
            }
            var points = input[input.Length - 1].Split(' ').Select(x => int.Parse(x)).ToArray();

            // Dijkstra's shortest path
            var distance = GetDijkstraDistance(vertices, points[0], points[1]);
            return distance == int.MaxValue ? "-1" : distance.ToString();
        }

        static void Main(string[] args)
        {
            var nmLine = Console.ReadLine();
            var edgeCount = int.Parse(nmLine.Split(' ').Last());
            var data = new string[edgeCount + 2];
            data[0] = nmLine;
            for (int i = 0; i < edgeCount; i++)
            {
                data[i + 1] = Console.ReadLine();
            }
            data[edgeCount + 1] = Console.ReadLine();
            Console.WriteLine(Solve(data));
        }
    }
}
