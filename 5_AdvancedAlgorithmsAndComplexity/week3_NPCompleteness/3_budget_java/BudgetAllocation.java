import java.io.*;
import java.util.*;

public class BudgetAllocation {
    private final InputReader reader;
    private final OutputWriter writer;

    public BudgetAllocation(InputReader reader, OutputWriter writer) {
        this.reader = reader;
        this.writer = writer;
    }

    public static void main(String[] args) {
        InputReader reader = new InputReader(System.in);
        OutputWriter writer = new OutputWriter(System.out);
        new BudgetAllocation(reader, writer).run();
        writer.writer.flush();
    }

    class ConvertILPToSat {
        int[][] A;
        int[] b;
        ArrayList<int[]> output;

        ConvertILPToSat(int n, int m) {
            A = new int[n][m];
            b = new int[n];
        }

        void printSat() {
            writer.printf("1 1\n");
            writer.printf("1 -1 0\n");
        }

        void printUnsat() {
            writer.printf("2 1\n");
            writer.printf("1 0\n");
            writer.printf("-1 0\n");
        }

        void printOutput() {
            for (int i = 0; i < output.size(); i++) {
                int[] clause = output.get(i);
                String[] stringVars = new String[clause.length];
                for (int j = 0; j < clause.length; j++)
                    stringVars[j] = Integer.toString(clause[j]);
                writer.printf(String.join(" ", stringVars) + "\n");
            }
        }
    }

    private static int[] ToBinary(int value, int digits) {
        int[] result = new int[digits];
        while (value > 0) {
            result[(int) Math.floor(Math.log10((value ^ (value - 1)) + 1) / Math.log10(2)) - 1] = 1;
            value &= (value - 1);
        }
        return result;
    }

    public void run() {
        int n = reader.nextInt();
        int m = reader.nextInt();

        ConvertILPToSat converter = new ConvertILPToSat(n, m);
        for (int i = 0; i < n; ++i) {
            for (int j = 0; j < m; ++j) {
                converter.A[i][j] = reader.nextInt();
            }
        }
        for (int i = 0; i < n; ++i) {
            converter.b[i] = reader.nextInt();
        }

        converter.output = new ArrayList<int[]>();
        converter.output.add(new int[] { 0, m });

        for (int i = 0; i < n; i++) {
            List<Integer> coeffs = new ArrayList<Integer>();
            for (int j = 0; j < m; j++) {
                if (converter.A[i][j] != 0)
                    coeffs.add(j);
            }
            if (coeffs.size() == 0 && converter.b[i] < 0) {
                converter.printUnsat();
                return;
            }
            // check any possible assignment of 3 vars
            int clauseCount = 0;
            for (int k = 0; k < Math.pow(2, coeffs.size()); k++) {
                int[] binary = ToBinary(k, coeffs.size());
                int sum = 0;
                for (int l = 0; l < binary.length; l++) {
                    sum += converter.A[i][coeffs.get(l)] * binary[l];
                }
                if (sum > converter.b[i]) {
                    // inequality falsified, add clause
                    int[] clause = new int[binary.length + 1];
                    clauseCount++;
                    if (clauseCount == Math.pow(2, coeffs.size())) {
                        converter.printUnsat();
                        return;
                    }
                    for (int y = 0; y < binary.length; y++) {
                        clause[y] = (binary[y] > 0 ? -1 : 1) * (coeffs.get(y) + 1);
                    }
                    converter.output.add(clause);
                }
            }
        }

        converter.output.get(0)[0] = converter.output.size() - 1;
        if (converter.output.size() == 1) {
            converter.printSat();
        } else {
            converter.printOutput();
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
