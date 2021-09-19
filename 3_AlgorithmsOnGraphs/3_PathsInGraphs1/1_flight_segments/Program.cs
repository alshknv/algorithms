using System.Collections.Generic;
using System;
using System.Linq;

namespace _1_flight_segments
{
    public class Vertex
    {
        public LinkedList<int> Destinations;
        public int CurrentDestination
        {
            get { return Destinations.First.Value; }
        }
        public int Distance = -1;
        public Vertex()
        {
            Destinations = new LinkedList<int>();
        }
        public void AddDestination(int destination)
        {
            Destinations.AddLast(destination);
        }
    }

    public static class FlightSegments
    {
        public static void BFS(Vertex[] vertices, int start)
        {
            var queue = new Queue<int>();
            vertices[start].Distance = 0;
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                var n = queue.Dequeue();
                while (vertices[n].Destinations.Count > 0)
                {
                    var destination = vertices[n].Destinations.First.Value;
                    if (vertices[destination].Distance < 0)
                    {
                        vertices[destination].Distance = vertices[n].Distance + 1;
                        queue.Enqueue(destination);
                    }
                    vertices[n].Destinations.RemoveFirst();
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
                vertices[edgeInfo[0]].AddDestination(edgeInfo[1]); //undirected
                vertices[edgeInfo[1]].AddDestination(edgeInfo[0]);
            }
            var points = input[input.Length - 1].Split(' ').Select(x => int.Parse(x)).ToArray();

            // breadth first search
            BFS(vertices, points[0]);
            return vertices[points[1]].Distance.ToString();
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
