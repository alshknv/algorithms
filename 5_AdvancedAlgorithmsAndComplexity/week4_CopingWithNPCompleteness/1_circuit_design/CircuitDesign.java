import java.io.*;
import java.util.*;
import java.util.Arrays;
import java.util.Locale;
import java.util.StringTokenizer;

public class CircuitDesign {
    private final InputReader reader;
    private final OutputWriter writer;

    public CircuitDesign(InputReader reader, OutputWriter writer) {
        this.reader = reader;
        this.writer = writer;
    }

    public static void main(String[] args) {
        InputReader reader = new InputReader(System.in);
        OutputWriter writer = new OutputWriter(System.out);
        new CircuitDesign(reader, writer).run();
        writer.writer.flush();
    }

    class ImplicationVertex {
        boolean visited;
        Integer preVisit;
        Integer postVisit;
        List<Integer> edges = new ArrayList<Integer>();
        List<Integer> edgesR = new ArrayList<Integer>();
        int currentEdge;
        int currentEdgeR;
    }

    class OrderedVertex {
        Integer order;
        int index;

        OrderedVertex(Integer ord, int idx) {
            order = ord;
            index = idx;
        }
    }

    class Clause {
        int firstVar;
        int secondVar;
    }

    class TwoSatisfiability {
        int numVars;
        Clause[] clauses;
        ImplicationVertex[] implicationGraph;
        Integer[] result;

        TwoSatisfiability(int n, int m) {
            numVars = n;
            clauses = new Clause[m];
            for (int i = 0; i < m; ++i) {
                clauses[i] = new Clause();
            }
        }

        int DFS(int index, int counter) {
            Stack<Integer> vertexStack = new Stack<Integer>();
            vertexStack.push(index);
            while (vertexStack.size() > 0) {
                int vx = vertexStack.peek();
                if (implicationGraph[vx].preVisit == null)
                    implicationGraph[vx].preVisit = counter++;
                while (implicationGraph[vx].currentEdgeR < implicationGraph[vx].edgesR.size() &&
                        implicationGraph[implicationGraph[vx].edgesR
                                .get(implicationGraph[vx].currentEdgeR)].preVisit != null) {
                    implicationGraph[vx].currentEdgeR++;
                }
                if (implicationGraph[vx].currentEdgeR >= implicationGraph[vx].edgesR.size()) {
                    vertexStack.pop();
                    implicationGraph[vx].postVisit = counter++;
                } else {
                    vertexStack.push(implicationGraph[vx].edgesR.get(implicationGraph[vx].currentEdgeR));
                }
            }
            return counter;
        }

        Boolean ExploreSCC(int index) {
            int id0 = (implicationGraph.length - 1) / 2;
            boolean[] vars = new boolean[id0];
            Stack<Integer> vertexStack = new Stack<Integer>();
            vertexStack.push(index);
            while (vertexStack.size() > 0) {
                int ve = vertexStack.peek();

                implicationGraph[ve].visited = true;
                if (implicationGraph[ve].currentEdge >= implicationGraph[ve].edges.size()) {
                    ve = vertexStack.pop();
                    int varIdx = Math.abs(ve - id0) - 1;
                    if (vars[varIdx])
                        return false;
                    vars[varIdx] = true;
                    if (result[ve] == null) {
                        result[ve] = 1;
                        result[-ve + 2 * id0] = -1;
                    }
                } else {
                    while (implicationGraph[ve].currentEdge < implicationGraph[ve].edges.size() &&
                            implicationGraph[implicationGraph[ve].edges
                                    .get(implicationGraph[ve].currentEdge)].visited) {
                        implicationGraph[ve].currentEdge++;
                    }
                    if (implicationGraph[ve].currentEdge < implicationGraph[ve].edges.size()) {
                        vertexStack.push(implicationGraph[ve].edges.get(implicationGraph[ve].currentEdge));
                    }
                }
            }
            return true;
        }

        Boolean isSatisfiable() {
            implicationGraph = new ImplicationVertex[numVars * 2 + 1];
            result = new Integer[numVars * 2 + 1];
            for (int i = 0; i < implicationGraph.length; i++)
                implicationGraph[i] = new ImplicationVertex();
            for (Clause clause : clauses) {
                if (clause.secondVar == 0) {
                    implicationGraph[numVars - clause.firstVar].edges.add(numVars + clause.firstVar);
                    implicationGraph[numVars + clause.firstVar].edgesR.add(numVars - clause.firstVar);
                } else {
                    implicationGraph[numVars - clause.firstVar].edges.add(numVars + clause.secondVar);
                    implicationGraph[numVars + clause.secondVar].edgesR.add(numVars - clause.firstVar);
                    implicationGraph[numVars - clause.secondVar].edges.add(numVars + clause.firstVar);
                    implicationGraph[numVars + clause.firstVar].edgesR.add(numVars - clause.secondVar);
                }
            }

            // DFS reversed graph to get postvisit indexes
            int iv = 0;
            int counter = 0;
            while (iv < implicationGraph.length) {
                while (iv < implicationGraph.length && implicationGraph[iv].preVisit != null)
                    iv++;
                if (iv < implicationGraph.length && iv != numVars) {
                    counter = DFS(iv, counter);
                }
                iv++;
            }

            // finding strongly connected components
            OrderedVertex[] postInfo = new OrderedVertex[implicationGraph.length];
            for (int i = 0; i < implicationGraph.length; i++) {
                postInfo[i] = new OrderedVertex(implicationGraph[i].postVisit, i);
            }

            Arrays.sort(postInfo, new Comparator<OrderedVertex>() {

                public int compare(OrderedVertex v1, OrderedVertex v2) {
                    if (v1.order == null)
                        return -1;
                    if (v2.order == null)
                        return 1;
                    return -v1.order.compareTo(v2.order);
                }
            });

            iv = 1;
            while (iv < postInfo.length) {
                while (iv < postInfo.length && implicationGraph[postInfo[iv].index].visited)
                    iv++;
                if (iv < postInfo.length) {
                    if (!ExploreSCC(postInfo[iv].index))
                        return false;
                }
                iv++;
            }
            return true;
        }
    }

    public void run() {
        int n = reader.nextInt();
        int m = reader.nextInt();

        TwoSatisfiability twoSat = new TwoSatisfiability(n, m);
        for (int i = 0; i < m; ++i) {
            twoSat.clauses[i].firstVar = reader.nextInt();
            twoSat.clauses[i].secondVar = reader.nextInt();
        }

        if (twoSat.isSatisfiable()) {
            writer.printf("SATISFIABLE\n");
            for (int i = n + 1; i <= n * 2; ++i) {
                writer.printf("%d", twoSat.result[i] * (i - n));
                if (i < n * 2) {
                    writer.printf(" ");
                } else {
                    writer.printf("\n");
                }
            }
        } else {
            writer.printf("UNSATISFIABLE\n");
        }
    }

    static class InputReader {
        public BufferedReader reader;
        public StringTokenizer tokenizer;

        public InputReader(InputStream stream) {
            reader = new BufferedReader(new InputStreamReader(stream), 32768);
            tokenizer = null;
        }

        public String next() {
            while (tokenizer == null || !tokenizer.hasMoreTokens()) {
                try {
                    tokenizer = new StringTokenizer(reader.readLine());
                } catch (IOException e) {
                    throw new RuntimeException(e);
                }
            }
            return tokenizer.nextToken();
        }

        public int nextInt() {
            return Integer.parseInt(next());
        }

        public double nextDouble() {
            return Double.parseDouble(next());
        }

        public long nextLong() {
            return Long.parseLong(next());
        }
    }

    static class OutputWriter {
        public PrintWriter writer;

        OutputWriter(OutputStream stream) {
            writer = new PrintWriter(stream);
        }

        public void printf(String format, Object... args) {
            writer.print(String.format(Locale.ENGLISH, format, args));
        }
    }
}