using System.Collections.Generic;
using System.Linq;
using System;

namespace _4_reschedule_exams
{
    public class ImplicationVertex
    {
        public bool Visited;
        public int? PreVisit;
        public int? PostVisit;
        private int[] edges;
        private int[] edgesR;
        public HashSet<int> EdgesHash = new HashSet<int>();
        public HashSet<int> EdgesHashR = new HashSet<int>();

        public int[] Edges
        {
            get
            {
                if (edges == null) edges = EdgesHash.ToArray();
                return edges;
            }
        }
        public int[] EdgesR
        {
            get
            {
                if (edgesR == null) edgesR = EdgesHashR.ToArray();
                return edgesR;
            }
        }

        public int CurrentEdge;
        public int CurrentEdgeR;
    }

    public static class RescheduleExams
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
                while (implicationGraph[vx].CurrentEdgeR < implicationGraph[vx].EdgesR.Length &&
                        implicationGraph[implicationGraph[vx].EdgesR[implicationGraph[vx].CurrentEdgeR]].PreVisit != null)
                {
                    implicationGraph[vx].CurrentEdgeR++;
                }
                if (implicationGraph[vx].CurrentEdgeR >= implicationGraph[vx].EdgesR.Length)
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
                if (implicationGraph[ve].CurrentEdge >= implicationGraph[ve].Edges.Length)
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
                    while (implicationGraph[ve].CurrentEdge < implicationGraph[ve].Edges.Length &&
                        implicationGraph[implicationGraph[ve].Edges[implicationGraph[ve].CurrentEdge]].Visited)
                    {
                        implicationGraph[ve].CurrentEdge++;
                    }
                    if (implicationGraph[ve].CurrentEdge < implicationGraph[ve].Edges.Length)
                    {
                        vertexStack.Push(implicationGraph[ve].Edges[implicationGraph[ve].CurrentEdge]);
                    }
                }
            }
            return true;
        }

        public static string Solve(string[] input)
        {
            var nm = input[0].Split(' ').Select(x => int.Parse(x)).ToArray();
            var rgb = input[1];

            var x = nm[0] * 3;
            implicationGraph = new ImplicationVertex[x * 2 + 1];
            assignment = new bool?[x * 2 + 1];

            for (int i = 0; i < implicationGraph.Length; i++)
                implicationGraph[i] = new ImplicationVertex();



            for (int j = 1; j <= nm[0]; j++)
            {
                int[] varIdx = null;
                switch (rgb[j - 1])
                {
                    case 'R':
                        varIdx = new int[] { 0, 1, 2 };
                        break;
                    case 'G':
                        varIdx = new int[] { 1, 0, 2 };
                        break;
                    case 'B':
                        varIdx = new int[] { 2, 0, 1 };
                        break;
                }

                // vertex must change color
                implicationGraph[x + (varIdx[0] * nm[0] + j)].EdgesHash.Add(x - (varIdx[0] * nm[0] + j));
                implicationGraph[x - (varIdx[0] * nm[0] + j)].EdgesHashR.Add(x + (varIdx[0] * nm[0] + j));

                // vertex must have one of other colors
                implicationGraph[x - (varIdx[1] * nm[0] + j)].EdgesHash.Add(x + (varIdx[2] * nm[0] + j));
                implicationGraph[x + (varIdx[2] * nm[0] + j)].EdgesHashR.Add(x - (varIdx[1] * nm[0] + j));
                implicationGraph[x - (varIdx[2] * nm[0] + j)].EdgesHash.Add(x + (varIdx[1] * nm[0] + j));
                implicationGraph[x + (varIdx[1] * nm[0] + j)].EdgesHashR.Add(x - (varIdx[2] * nm[0] + j));

                // vertex must have only one of other colors
                implicationGraph[x + (varIdx[1] * nm[0] + j)].EdgesHash.Add(x - (varIdx[2] * nm[0] + j));
                implicationGraph[x - (varIdx[2] * nm[0] + j)].EdgesHashR.Add(x + (varIdx[1] * nm[0] + j));
                implicationGraph[x + (varIdx[2] * nm[0] + j)].EdgesHash.Add(x - (varIdx[1] * nm[0] + j));
                implicationGraph[x - (varIdx[1] * nm[0] + j)].EdgesHashR.Add(x + (varIdx[2] * nm[0] + j));
            }
            for (int i = 2; i < input.Length; i++)
            {
                var edge = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                implicationGraph[x + edge[0]].EdgesHash.Add(x - edge[1]);
                implicationGraph[x - edge[1]].EdgesHashR.Add(x + edge[0]);
                implicationGraph[x + edge[1]].EdgesHash.Add(x - edge[0]);
                implicationGraph[x - edge[0]].EdgesHashR.Add(x + edge[1]);

                implicationGraph[x + nm[0] + edge[0]].EdgesHash.Add(x - (nm[0] + edge[1]));
                implicationGraph[x - (nm[0] + edge[1])].EdgesHashR.Add(x + nm[0] + edge[0]);
                implicationGraph[x + nm[0] + edge[1]].EdgesHash.Add(x - (nm[0] + edge[0]));
                implicationGraph[x - (nm[0] + edge[0])].EdgesHashR.Add(x + nm[0] + edge[1]);
                implicationGraph[x + nm[0] * 2 + edge[0]].EdgesHash.Add(x - (nm[0] * 2 + edge[1]));
                implicationGraph[x - (nm[0] * 2 + edge[1])].EdgesHashR.Add(x + nm[0] * 2 + edge[0]);
                implicationGraph[x + nm[0] * 2 + edge[1]].EdgesHash.Add(x - (nm[0] * 2 + edge[0]));
                implicationGraph[x - (nm[0] * 2 + edge[0])].EdgesHashR.Add(x + nm[0] * 2 + edge[1]);
            }

            // DFS reversed graph to get postvisit indexes
            var iv = 0;
            var counter = 0;
            while (iv < implicationGraph.Length)
            {
                while (iv < implicationGraph.Length && implicationGraph[iv].PreVisit != null) iv++;
                if (iv < implicationGraph.Length && iv != nm[0] * 3)
                {
                    counter = DFS(iv, counter);
                }
                iv++;
            }

            // finding strongly connected components
            var postInfo = implicationGraph.Select((vrt, i) => new KeyValuePair<int, int?>(i, vrt?.PostVisit)).ToArray();
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
                    if (!ExploreSCC(postInfo[iv].Key)) return "Impossible";
                }
                iv++;
            }
            return "";
        }

        static void Main(string[] args)
        {
            var gg = Solve(new string[] {"4 5",
"RRRG",
"1 3",
"1 4",
"3 4",
"2 4",
"2 3"});
            return;
            var nmline = Console.ReadLine();
            var m = int.Parse(nmline.Split(' ').Last());
            var input = new string[m + 2];
            input[0] = nmline;
            for (int i = 1; i <= m + 1; i++)
            {
                input[i] = Console.ReadLine();
            }
            Console.WriteLine(Solve(input));
        }
    }
}
