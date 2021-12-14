using System.Collections.Generic;
using System;
using System.Linq;

namespace _1_phiX174_error_free
{
    public static class Overlap
    {
        private static int[] SortChars(string text)
        {
            var order = new int[text.Length];
            var count = new SortedDictionary<char, int>();
            for (int i = 0; i < text.Length; i++)
                count[text[i]] = (count.ContainsKey(text[i]) ? count[text[i]] : 0) + 1;
            var keys = count.Keys.ToArray();
            for (int j = 1; j < keys.Length; j++)
                count[keys[j]] += count[keys[j - 1]];
            for (int k = text.Length - 1; k >= 0; k--)
            {
                var ch = text[k];
                count[ch]--;
                order[count[ch]] = k;
            }
            return order;
        }

        private static int[] ComputeClasses(string text, int[] order)
        {
            var classArr = new int[text.Length];
            classArr[order[0]] = 0;
            for (int i = 1; i < text.Length; i++)
            {
                classArr[order[i]] = classArr[order[i - 1]] + (text[order[i]] != text[order[i - 1]] ? 1 : 0);
            }
            return classArr;
        }

        private static int[] Sort2L(int textlen, int l, int[] order, int[] classArr)
        {
            var count = new int[textlen];
            var newOrder = new int[textlen];
            for (int i = 0; i < textlen; i++)
                count[classArr[i]]++;
            for (int j = 1; j < textlen; j++)
                count[j] += count[j - 1];
            for (int k = textlen - 1; k >= 0; k--)
            {
                var start = (order[k] - l + textlen) % textlen;
                var cl = classArr[start];
                count[cl]--;
                newOrder[count[cl]] = start;
            }
            return newOrder;
        }

        private static int[] UpdateClasses(int[] newOrder, int[] classArr, int l)
        {
            var newClass = new int[newOrder.Length];
            newClass[newOrder[0]] = 0;
            for (int i = 1; i < newOrder.Length; i++)
            {
                var current = newOrder[i];
                var previous = newOrder[i - 1];
                var middle = (current + l) % newOrder.Length;
                var middlePrev = (previous + l) % newOrder.Length;
                newClass[current] = newClass[previous] + (classArr[current] != classArr[previous] || classArr[middle] != classArr[middlePrev] ? 1 : 0);
            }
            return newClass;
        }

        private static int Lcp(string s, int i, int j)
        {
            var lcp = 0;
            while (i + lcp < s.Length && j + lcp < s.Length)
            {
                if (s[i + lcp] == s[j + lcp])
                    lcp++;
                else break;
            }
            return lcp;
        }

        public static int FindLongest(string read1, string read2)
        {
            var text = read1 + "#" + read2 + "$";
            var order = SortChars(text);
            var classArr = ComputeClasses(text, order);
            var textlen = text.Length;
            var l = 1;
            while (l < textlen)
            {
                order = Sort2L(textlen, l, order, classArr);
                classArr = UpdateClasses(order, classArr, l);
                l = 2 * l;
            }
            var len1 = (text.Length - 2) / 2;
            var maxOver = 0;
            for (int i = 2; i < order.Length; i++)
            {
                for (int j = 2; j < order.Length; j++)
                {
                    if (i == j) continue;
                    var suffLen = len1 - order[i];
                    if (suffLen > 0 && order[i] < len1 && order[j] == len1 + 1 && Lcp(text, order[i], order[j]) == suffLen)
                    {
                        if (suffLen > maxOver) maxOver = suffLen;
                    }
                }
            }
            return maxOver;
        }
    }

    public class Edge
    {
        public int Destination;
        public int Overlap;
    }

    public class Vertex
    {
        public string Read;
        public bool Visited;
        public int Id;
        public LinkedList<Edge> Edges = new LinkedList<Edge>();
        public LinkedListNode<Edge> CurrentEdge;

        public Vertex(int id, string read)
        {
            Id = id;
            Read = read;
        }
        public void AddEdge(int overlap, int destination)
        {
            var edge = Edges.First;
            var newEdge = new LinkedListNode<Edge>(new Edge() { Overlap = overlap, Destination = destination });
            if (edge == null)
            {
                Edges.AddFirst(newEdge);
            }
            else
            {
                while (edge != null && edge.Value.Overlap > overlap) edge = edge.Next;
                if (edge == null)
                {
                    Edges.AddLast(newEdge);
                }
                else
                {
                    Edges.AddBefore(edge, newEdge);
                }
            }
        }
    }

    public static class ErrorFree
    {
        public static string Assemble(string[] reads)
        {
            var overlapGraph = new Vertex[reads.Length];

            // build overlap graph
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = i + 1; j < reads.Length; j++)
                {
                    var overlapIJ = Overlap.FindLongest(reads[i], reads[j]);
                    if (overlapGraph[i] == null) overlapGraph[i] = new Vertex(i, reads[i]);
                    if (overlapIJ > 0)
                        overlapGraph[i].AddEdge(overlapIJ, j);
                    var overlapJI = Overlap.FindLongest(reads[j], reads[i]);
                    if (overlapGraph[j] == null) overlapGraph[j] = new Vertex(j, reads[j]);
                    if (overlapJI > 0)
                        overlapGraph[j].AddEdge(overlapJI, i);
                }
            }

            for (int i = 0; i < overlapGraph.Length; i++)
            {
                overlapGraph[i].CurrentEdge = overlapGraph[i].Edges.First;
            }

            // greedy hamiltonian path
            var path = new List<Vertex>(overlapGraph.Length) { overlapGraph[0] };
            var overlaps = new List<int>(overlapGraph.Length);
            do
            {
                var vertex = path[path.Count - 1];
                vertex.Visited = true;
                while (vertex.CurrentEdge != null && overlapGraph[vertex.CurrentEdge.Value.Destination].Visited)
                    vertex.CurrentEdge = vertex.CurrentEdge.Next;
                if (vertex.CurrentEdge == null)
                {
                    overlapGraph[vertex.Id].Visited = false;
                    path.RemoveAt(path.Count - 1);
                    if (path.Count > 0)
                    {
                        path[path.Count - 1].CurrentEdge = path[path.Count - 1].CurrentEdge.Next;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    path.Add(overlapGraph[vertex.CurrentEdge.Value.Destination]);
                    overlaps.Add(vertex.CurrentEdge.Value.Overlap);
                    if (path.Count == overlapGraph.Length) break;
                }
            } while (true);

            var genome = new string[overlapGraph.Length];
            genome[0] = path[0].Read;
            for (int i = 1; i < overlapGraph.Length; i++)
            {
                genome[i] = path[i].Read.Substring(overlaps[i - 1]);
            }
            var lastVertex = overlapGraph[path[path.Count - 1].Id];
            while (lastVertex.CurrentEdge?.Value.Destination != 0) lastVertex.CurrentEdge = lastVertex.CurrentEdge.Next;
            var cycleOverlap = lastVertex.CurrentEdge?.Value.Overlap ?? 0;
            genome[genome.Length - 1] = genome[genome.Length - 1].Substring(0, cycleOverlap / 2);
            genome[0] = genome[0].Substring(cycleOverlap - cycleOverlap / 2);
            return string.Concat(genome);
        }

        static void Main(string[] args)
        {
            var gg = Assemble(new string[] { "AAC", "ACG", "GAA", "GTT", "TCG" });
            return;
            var reads = new string[1618];
            for (int i = 0; i < 1618; i++)
                reads[i] = Console.ReadLine();
            Console.WriteLine(Assemble(reads));
        }
    }
}
