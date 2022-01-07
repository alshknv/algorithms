using System.Linq;
using System.Collections.Generic;
using System;

namespace _5_phi174_error_prone
{
    public class Vertex
    {
        public int Index;
        public bool Deleted;
        public string Mer;
        public List<Edge> OutgoingEdges = new List<Edge>();
        public List<Edge> IncomingEdges = new List<Edge>();
        public int VisitedCount;
        public bool AllOutgoingVisited
        {
            get
            {
                return VisitedCount == OutgoingEdges.Count;
            }
        }
        public Vertex(int index, string mer)
        {
            Index = index;
            Mer = mer;
        }
        public void AddOutgoingEdge(int destination)
        {
            OutgoingEdges.Add(new Edge(destination));
        }
        public void AddIncomingEdge(int destination)
        {
            IncomingEdges.Add(new Edge(destination));
        }
    }

    public class Edge
    {
        public int Connection;
        public bool Visited;
        public Edge(int connection)
        {
            Connection = connection;
        }
    }

    public class Graph
    {
        private readonly List<string> mers = new List<string>();
        public readonly Dictionary<string, Vertex> Vertices = new Dictionary<string, Vertex>();
        public int VertexCount
        {
            get { return Vertices.Keys.Count; }
        }

        public Vertex this[int index]
        {
            get
            {
                return Vertices[mers[index]];
            }
            set
            {
                Vertices[mers[index]] = value;
            }
        }

        public int Vertex(string mer)
        {
            Vertex vertex;
            if (!mers.Contains(mer))
            {
                vertex = new Vertex(mers.Count, mer);
                mers.Add(mer);
                Vertices.Add(mer, vertex);
            }
            else
            {
                vertex = Vertices[mer];
            }
            return vertex.Index;
        }
    }

    public static class Phi174ErrorProne
    {
        private static Graph graph;
        private static HashSet<string> GetKMers(string[] reads, int k)
        {
            var kmers = new HashSet<string>();
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = 0; j <= reads[i].Length - k; j++)
                {
                    kmers.Add(reads[i].Substring(j, k));
                }
            }
            return kmers;
        }

        private static int ConstructGraph(string[] kmers)
        {
            graph = new Graph();

            // construct De Bruijn graph
            var edgeCount = 0;
            for (int i = 0; i < kmers.Length; i++)
            {
                var from = graph.Vertex(kmers[i].Substring(0, kmers[i].Length - 1));
                var to = graph.Vertex(kmers[i].Substring(1, kmers[i].Length - 1));
                graph[from].AddOutgoingEdge(to);
                graph[to].AddIncomingEdge(from);
                edgeCount++;
            }
            return edgeCount;
        }

        private static void RemoveTips()
        {
            int tipCount;
            do
            {
                tipCount = 0;
                for (int i = 0; i < graph.VertexCount; i++)
                {
                    if (graph[i].Deleted) continue;
                    if (graph[i].IncomingEdges.Count == 0)
                    {
                        // source tip
                        tipCount += graph[i].OutgoingEdges.Count;
                        for (int j = 0; j < graph[i].OutgoingEdges.Count; j++)
                        {
                            graph[graph[i].OutgoingEdges[j].Connection].IncomingEdges.RemoveAll(x => x.Connection == i);
                        }
                        graph[i].Deleted = true;
                        i = -1;
                    }
                    if (graph[i].OutgoingEdges.Count == 0)
                    {
                        // target tip
                        tipCount += graph[i].IncomingEdges.Count;
                        for (int j = 0; j < graph[i].IncomingEdges.Count; j++)
                        {
                            graph[graph[i].IncomingEdges[j].Connection].OutgoingEdges.RemoveAll(x => x.Connection == i);
                        }
                        graph[i].Deleted = true;
                        i = -1;
                    }
                }
            } while (tipCount > 0);
        }

        private static bool FindCycle(int start, out List<int> cycle)
        {
            var v = start;
            cycle = new List<int>() { start };
            do
            {
                // continue to go through unvisited edges until we come back to start
                var edgeFound = false;
                for (int e = 0; e < graph[v].OutgoingEdges.Count; e++)
                {
                    if (!graph[v].OutgoingEdges[e].Visited && !graph[graph[v].OutgoingEdges[e].Connection].Deleted)
                    {
                        graph[v].OutgoingEdges[e].Visited = true;
                        graph[v].VisitedCount++;
                        v = graph[v].OutgoingEdges[e].Connection;
                        if (v != start)
                            cycle.Add(v);
                        edgeFound = true;
                        break;
                    }
                }
                if (!edgeFound) return false;
            } while (v != start);
            return true;
        }

        private static void DFS(int start, int[] targets, int v, int maxdepth, List<int> path, Dictionary<Tuple<int, int>, List<List<int>>> bubbleCandidates)
        {
            if (graph[v].Deleted) return;
            if (v != start && targets.Contains(v))
            {
                // vertex with multiple incoming edges reached
                var tuple = new Tuple<int, int>(start, v);
                if (bubbleCandidates.ContainsKey(tuple))
                    bubbleCandidates[tuple].Add(path);
                else
                    bubbleCandidates.Add(tuple, new List<List<int>>() { path });
            }
            if (path.Count > maxdepth) return;
            foreach (var e in graph[v].OutgoingEdges)
            {
                var newPath = new List<int>(path)
                {
                    e.Connection
                };
                DFS(start, targets, e.Connection, maxdepth, newPath, bubbleCandidates);
            }
        }

        private static void HandleBubbles(string[] kmers)
        {
            var multipleIn = new List<int>();
            var multipleOut = new List<int>();
            for (int i = 0; i < graph.VertexCount; i++)
            {
                if (graph[i].IncomingEdges.Count > 1) multipleIn.Add(i);
                if (graph[i].OutgoingEdges.Count > 1) multipleOut.Add(i);
            }

            // for each pair of mult-out & mult-in nodes find all paths between them shorter than threshold
            var bubbleCandidates = new Dictionary<Tuple<int, int>, List<List<int>>>();
            var vertices = graph.Vertices.Select(x => x.Value).ToArray();
            for (int i = 0; i < multipleOut.Count; i++)
            {
                DFS(multipleOut[i], multipleIn.ToArray(), multipleOut[i], kmers[0].Length, new List<int>() { multipleOut[i] }, bubbleCandidates);
            }

            // find all pairs of non-overlapping disjoint paths
            foreach (var pair in bubbleCandidates.Keys.ToArray())
            {
                for (int i = 0; i < bubbleCandidates[pair].Count; i++)
                {
                    for (int j = i + 1; j < bubbleCandidates[pair].Count; j++)
                    {
                        var path1 = bubbleCandidates[pair][i];
                        var path2 = bubbleCandidates[pair][j];
                        if (path1.GroupBy(x => x).Count() < path1.Count) continue;
                        if (path2.GroupBy(x => x).Count() < path2.Count) continue;
                        if (!path1.Skip(1).Take(bubbleCandidates[pair][i].Count - 2).Intersect(path2.Skip(1).Take(bubbleCandidates[pair][j].Count - 2)).Any())
                        {
                            var path = path2.Count > path1.Count ? path1 : path2;
                            for (int k = 1; k < path.Count - 1; k++)
                            {
                                if (graph[path[k]].Deleted) continue;
                                for (int l = 0; l < graph[path[k]].OutgoingEdges.Count; l++)
                                {
                                    graph[graph[path[k]].OutgoingEdges[l].Connection].IncomingEdges.RemoveAll(x => x.Connection == k);
                                }
                                graph[path[k]].Deleted = true;
                            }
                        }
                    }
                }
            }
        }

        private static List<int> GetCycle()
        {
            // find first cycle
            List<int> cycle;
            FindCycle(0, out cycle);

            // continue while cycle length is less than total number of edges

            while (true)
            {
                var hasUnexplored = false;
                for (int i = 0; i < cycle.Count; i++)
                {
                    if (graph[cycle[i]].AllOutgoingVisited) continue;
                    for (int j = 0; j < graph[cycle[i]].OutgoingEdges.Count; j++)
                    {
                        if (!graph[cycle[i]].OutgoingEdges[j].Visited)
                        {
                            // unvisited edge found
                            hasUnexplored = true;
                            List<int> nextCycle;
                            FindCycle(cycle[i], out nextCycle);

                            // insert new cycle in the middle of the old one and check for unvisited edges again
                            var newCycle = cycle.Take(i).ToList();
                            newCycle.AddRange(nextCycle);
                            newCycle.AddRange(cycle.Skip(i).Take(cycle.Count - i));
                            cycle = newCycle;
                            break;
                        }
                    }
                }
                if (!hasUnexplored) break;
            }
            return cycle;
        }

        public static string Assemble(string[] reads)
        {
            var kmers = GetKMers(reads, reads[0].Length < 5 ? 3 : 19).ToArray();
            ConstructGraph(kmers);
            RemoveTips();
            HandleBubbles(kmers);
            var cycle = GetCycle();
            //var cycle = HandleBubbles();
            // compose genome
            var genomeArray = new char[cycle.Count];
            for (int c = 0; c < cycle.Count; c++)
            {
                var vertexMer = graph[cycle[c]].Mer;
                genomeArray[c] = vertexMer[vertexMer.Length - 1]; //take last nucleotide from only, others are overlapped
            }
            return string.Concat(genomeArray);
        }

        static void Main(string[] args)
        {
            var input = new List<string>();
            string inLine;
            while ((inLine = Console.ReadLine()) != null)
            {
                input.Add(inLine);
            }
            Console.WriteLine(Assemble(input.ToArray()));
        }
    }
}
