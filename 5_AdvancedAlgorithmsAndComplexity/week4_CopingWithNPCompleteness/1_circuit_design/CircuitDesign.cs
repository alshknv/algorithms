using System.Collections.Generic;
using System.Linq;
using System;

namespace _1_circuit_design
{
    public class ImplicationVertex
    {
        public bool Visited;
        public int? PreVisit;
        public int? PostVisit;
        public List<int> Edges = new List<int>();
        public List<int> EdgesR = new List<int>();
        public int CurrentEdge;
        public int CurrentEdgeR;
    }

    public static class CircuitDesign
    {
        private static ImplicationVertex[] implicationGraph;
        private static bool?[] assignment;

        private static int DFS(int index, int counter)
        {
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var vx = vertexStack.Peek();
                if (implicationGraph[vx].PreVisit == null) implicationGraph[vx].PreVisit = counter++;
                while (implicationGraph[vx].CurrentEdgeR < implicationGraph[vx].EdgesR.Count &&
                        implicationGraph[implicationGraph[vx].EdgesR[implicationGraph[vx].CurrentEdgeR]].PreVisit != null)
                {
                    implicationGraph[vx].CurrentEdgeR++;
                }
                if (implicationGraph[vx].CurrentEdgeR >= implicationGraph[vx].EdgesR.Count)
                {
                    vertexStack.Pop();
                    implicationGraph[vx].PostVisit = counter++;
                }
                else
                {
                    vertexStack.Push(implicationGraph[vx].EdgesR[implicationGraph[vx].CurrentEdgeR]);
                }
            }
            return counter;
        }

        private static bool ExploreSCC(int index)
        {
            var id0 = (implicationGraph.Length - 1) / 2;
            var vars = new bool[id0];
            var vertexStack = new Stack<int>();
            vertexStack.Push(index);
            while (vertexStack.Count > 0)
            {
                var ve = vertexStack.Peek();

                implicationGraph[ve].Visited = true;
                if (implicationGraph[ve].CurrentEdge >= implicationGraph[ve].Edges.Count)
                {
                    ve = vertexStack.Pop();
                    var varIdx = Math.Abs(ve - id0) - 1;
                    if (vars[varIdx]) return false;
                    vars[varIdx] = true;
                    if (assignment[ve] == null)
                    {
                        assignment[ve] = true;
                        assignment[-ve + 2 * id0] = false;
                    }
                }
                else
                {
                    while (implicationGraph[ve].CurrentEdge < implicationGraph[ve].Edges.Count &&
                        implicationGraph[implicationGraph[ve].Edges[implicationGraph[ve].CurrentEdge]].Visited)
                    {
                        implicationGraph[ve].CurrentEdge++;
                    }
                    if (implicationGraph[ve].CurrentEdge < implicationGraph[ve].Edges.Count)
                    {
                        vertexStack.Push(implicationGraph[ve].Edges[implicationGraph[ve].CurrentEdge]);
                    }
                }
            }
            return true;
        }

        public static string[] Solve(string[] input)
        {
            var vc = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            // init implication graph
            implicationGraph = new ImplicationVertex[vc[0] * 2 + 1];
            assignment = new bool?[vc[0] * 2 + 1];
            for (int i = 0; i < implicationGraph.Length; i++)
                implicationGraph[i] = new ImplicationVertex();
            for (int i = 1; i < input.Length; i++)
            {
                var clause = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                if (clause.Length == 1)
                {
                    implicationGraph[vc[0] - clause[0]].Edges.Add(vc[0] + clause[0]);
                    implicationGraph[vc[0] + clause[0]].EdgesR.Add(vc[0] - clause[0]);
                }
                else
                {
                    implicationGraph[vc[0] - clause[0]].Edges.Add(vc[0] + clause[1]);
                    implicationGraph[vc[0] + clause[1]].EdgesR.Add(vc[0] - clause[0]);
                    implicationGraph[vc[0] - clause[1]].Edges.Add(vc[0] + clause[0]);
                    implicationGraph[vc[0] + clause[0]].EdgesR.Add(vc[0] - clause[1]);
                }
            }

            // DFS reversed graph to get postvisit indexes
            var iv = 0;
            var counter = 0;
            while (iv < implicationGraph.Length)
            {
                while (iv < implicationGraph.Length && implicationGraph[iv].PreVisit != null) iv++;
                if (iv < implicationGraph.Length && iv != vc[0])
                {
                    counter = DFS(iv, counter);
                }
                iv++;
            }

            // finding strongly connected components
            var postInfo = implicationGraph.Select((vrt, i) => new KeyValuePair<int, int?>(i, vrt.PostVisit)).ToArray();
            Array.Sort(postInfo, (p1, p2) =>
            {
                if (p1.Value == null) return -1;
                if (p2.Value == null) return 1;
                return -((int)p1.Value).CompareTo((int)p2.Value);
            });

            iv = 1;
            while (iv < postInfo.Length)
            {
                while (iv < postInfo.Length && implicationGraph[postInfo[iv].Key].Visited) iv++;
                if (iv < postInfo.Length)
                {
                    if (!ExploreSCC(postInfo[iv].Key)) return new string[] { "UNSATISFIABLE" };
                }
                iv++;
            }

            return new string[] { "SATISFIABLE", string.Join(" ", assignment.Skip(vc[0] + 1).Take(vc[0]).Select((a, i) => ((bool)a ? 1 : -1) * (i + 1))) };
        }

        static void Main(string[] args)
        {
            var vcline = Console.ReadLine();
            var c = int.Parse(vcline.Split(' ')[1]);
            var input = new string[c + 1];
            input[0] = vcline;
            for (int i = 1; i <= c; i++)
            {
                input[i] = Console.ReadLine();
            }
            foreach (var line in Solve(input))
                Console.WriteLine(line);
        }
    }
}
