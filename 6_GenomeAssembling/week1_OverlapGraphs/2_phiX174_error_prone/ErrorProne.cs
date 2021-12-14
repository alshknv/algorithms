using System.Collections.Generic;
using System;
using System.Linq;

namespace _2_phiX174_error_prone
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

        private static int Lcp(string s, int i, int j, int maxErrors)
        {
            var lcp = 0;
            var errors = 0;
            while (i + lcp < s.Length && j + lcp < s.Length)
            {
                if (s[i + lcp] == s[j + lcp])
                {
                    lcp++;
                }
                else
                {
                    errors++;
                    if (errors <= maxErrors) lcp++;
                    else break;
                }
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
                    if (suffLen > 0 && order[i] < len1 && order[j] == len1 + 1 && Lcp(text, order[i], order[j], 1) == suffLen)
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
        public LinkedList<Edge> Edges = new LinkedList<Edge>();

        public Vertex(string read)
        {
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
    public static class ErrorProne
    {
        public static string Assemble(string[] reads)
        {
            var overlapGraph = new Vertex[reads.Length];
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = i + 1; j < reads.Length; j++)
                {
                    var overlapIJ = Overlap.FindLongest(reads[i], reads[j]);
                    if (overlapGraph[i] == null) overlapGraph[i] = new Vertex(reads[i]);
                    overlapGraph[i].AddEdge(overlapIJ, j);
                    var overlapJI = Overlap.FindLongest(reads[j], reads[i]);
                    if (overlapGraph[j] == null) overlapGraph[j] = new Vertex(reads[j]);
                    overlapGraph[j].AddEdge(overlapJI, i);
                }
            }
            return "";
        }

        static void Main(string[] args)
        {
            var gg = Assemble(new string[] { "GTT", "TAG" });
            return;
            var reads = new string[1618];
            for (int i = 0; i < 1618; i++)
                reads[i] = Console.ReadLine();
            Console.WriteLine(Assemble(reads));
        }
    }
}
