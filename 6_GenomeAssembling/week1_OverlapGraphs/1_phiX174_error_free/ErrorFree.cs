using System.Collections.Generic;
using System;
using System.Linq;

namespace _1_phiX174_error_free
{
    public class StNode
    {
        public StNode Parent;
        public SortedDictionary<char, StNode> Children = new SortedDictionary<char, StNode>();
        public int Depth;
        public int Start;
        public int End;
    }

    public static class SuffixTree
    {
        private static StNode CreateNewLeaf(StNode node, string s, int suffix)
        {
            var leaf = new StNode()
            {
                Parent = node,
                Depth = s.Length - suffix,
                Start = suffix + node.Depth,
                End = s.Length - 1
            };
            node.Children[s[leaf.Start]] = leaf;
            return leaf;
        }

        private static StNode BreakEdge(StNode node, string s, int start, int offset)
        {
            var startCh = s[start];
            var midCh = s[start + offset];
            var midNode = new StNode()
            {
                Parent = node,
                Depth = node.Depth + offset,
                Start = start,
                End = start + offset - 1
            };
            midNode.Children[midCh] = node.Children[startCh];
            node.Children[startCh].Parent = midNode;
            node.Children[startCh].Start += offset;
            node.Children[startCh] = midNode;
            return midNode;
        }

        private static int[] InvertSuffixArray(int[] order)
        {
            var pos = new int[order.Length];
            for (int i = 0; i < pos.Length; i++)
            {
                pos[order[i]] = i;
            }
            return pos;
        }

        private static int Lcp(string s, int i, int j, int equal)
        {
            var lcp = Math.Max(0, equal);
            while (i + lcp < s.Length && j + lcp < s.Length)
            {
                if (s[i + lcp] == s[j + lcp])
                    lcp++;
                else break;
            }
            return lcp;
        }

        private static int[] LcpArray(string s, int[] order)
        {
            var lcpArray = new int[s.Length - 1];
            var lcp = 0;
            var posInOrder = InvertSuffixArray(order);
            var suffix = order[0];
            for (int i = 0; i < s.Length; i++)
            {
                var orderIndex = posInOrder[suffix];
                if (orderIndex == s.Length - 1)
                {
                    lcp = 0;
                    suffix = (suffix + 1) % s.Length;
                    continue;
                }
                var nextSuffix = order[orderIndex + 1];
                lcp = Lcp(s, suffix, nextSuffix, lcp - 1);
                lcpArray[orderIndex] = lcp;
                suffix = (suffix + 1) % s.Length;
            }
            return lcpArray;
        }

        public static StNode FromArray(string s, int[] order)
        {
            var lcpArray = LcpArray(s, order);
            var root = new StNode() { Start = -1, End = -1 };
            var lcpPrev = 0;
            var curNode = root;
            for (int i = 0; i < s.Length; i++)
            {
                var suffix = order[i];
                while (curNode.Depth > lcpPrev) curNode = curNode.Parent;
                if (curNode.Depth == lcpPrev)
                {
                    curNode = CreateNewLeaf(curNode, s, suffix);
                }
                else
                {
                    var edgeStart = order[i - 1] + curNode.Depth;
                    var offset = lcpPrev - curNode.Depth;
                    var midNode = BreakEdge(curNode, s, edgeStart, offset);
                    curNode = CreateNewLeaf(midNode, s, suffix);
                }
                if (i < s.Length - 1) lcpPrev = lcpArray[i];
            }
            return root;
        }
    }

    public static class SuffixArray
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

        public static int[] FromString(string s)
        {
            var order = SortChars(s);
            var classArr = ComputeClasses(s, order);
            var l = 1;
            while (l < s.Length)
            {
                order = Sort2L(s.Length, l, order, classArr);
                classArr = UpdateClasses(order, classArr, l);
                l = 2 * l;
            }
            return order;
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
        private static int FindLongestOverlap(string read1, string read2)
        {
            var s = read1 + "#" + read2 + "$";
            // build suffix array
            var order = SuffixArray.FromString(s);
            // build suffix tree
            var tree = SuffixTree.FromArray(s, order);


            // find max overlap

            int maxOverlap = 0;
            var stack = new Stack<StNode>();
            stack.Push(tree);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                //check if it is overlap node
                var hasSuffix = false;
                var hasPrefix = false;
                foreach (var child in node.Children)
                {
                    if (child.Value.Start == read1.Length)
                        hasSuffix = true;
                    else if (child.Value.Start > read2.Length && child.Value.Start - (read1.Length + 1) == node.Depth)
                        hasPrefix = true;
                    if (hasSuffix && hasPrefix)
                    {
                        // overlap found
                        if (node.Depth > maxOverlap) maxOverlap = node.Depth;
                    }
                    stack.Push(child.Value);
                }
            }
            return maxOverlap;
        }
        public static string Assemble(string[] reads)
        {
            var overlapGraph = new Vertex[reads.Length];

            // build overlap graph
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = i + 1; j < reads.Length; j++)
                {
                    var overlapIJ = FindLongestOverlap(reads[i], reads[j]);
                    if (overlapGraph[i] == null) overlapGraph[i] = new Vertex(i, reads[i]);
                    if (overlapIJ > 0)
                        overlapGraph[i].AddEdge(overlapIJ, j);
                    var overlapJI = FindLongestOverlap(reads[j], reads[i]);
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
