import java.util.*;

public class HeavyHitters {
    private static int p = 1000000007;
    private static int nh = 5;
    private static int nb = 100000;

    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        int n = scanner.nextInt();
        int t = scanner.nextInt();

        // init hash functions
        int[][] hash = new int[nh][];
        int a = 10;
        int b = 33;
        for (int i = 0; i < nh; i++)
        {
            hash[i] = new int[] { a, b };
            a *= 2;
            b *= 3;
        }

        // int stream data buf
        int[][] data = new int[nb][nh];

        // processing stream
        for (int i = 0; i < 2*n; ++i) {
            int l1 = scanner.nextInt();
            int l2 = scanner.nextInt();
            for (int j = 0; j < nh; j++) {
                data[HashM(hash[j],l1, nb)][j] += (i < n ? 1 : -1) * l2 * HashS(hash[j],l1);
            }
        }
        
        // processing queries
        int queries_n = scanner.nextInt();
        int[] result = new int[queries_n];
        int half2 = 0;
        int nh2 = nh / 2;
        int med;
        for (int i = 0; i < queries_n; i++)
        {
            int query = scanner.nextInt();
            PriorityQueue.Clear();
            for (int j = 0; j < nh; j++)
            {
                PriorityQueue.Add(data[HashM(hash[j], query, nb)][j] * HashS(hash[j], query));
            }
            // median
            for (int j = 0; j < nh2; j++)
                half2 = PriorityQueue.Extract();
            med = nh2 > 0 && nh2 % 2 > 0 ? PriorityQueue.Extract() : (PriorityQueue.Extract() + half2) / 2;
            result[i] = med < t ? 0 : 1;
        }
        scanner.close();
        
        for (int i = 0; i < result.length; ++i) 
        {
            System.out.print(result[i] + (i<result.length-1 ? " " : ""));
        }
    }

    private static int HashM(int[] f, long x, int m)
    {
        return (int)(((f[0] * x) + f[1]) % p % m);
    }

    private static int HashS(int[] f, int x)
    {
        double v1 = ((f[0] * x) + f[1]) % p / p;
        return 0.5 - v1 > 0 ? 1 : -1;
    }

    public static class PriorityQueue {
        private static int[] q = new int[100];
        private static int count = 0;

        private static void Swap(int x, int y) {
            int buf = q[x];
            q[x] = q[y];
            q[y] = buf;
        }

        private static void SiftUp(int i) {
            int p = (i - 1) / 2;
            while (i > 0 && q[i] > q[p]) {
                Swap(i, p);
                i = p;
                p = i / 2;
            }
        }

        private static void SiftDown(int i) {
            int l = 2 * i + 1;
            int r = 2 * i + 2;
            while (l < count) {
                if (r < count && q[r] > q[l])
                    l = r;
                if (q[l] > q[i]) {
                    Swap(i, l);
                    i = l;
                } else {
                    break;
                }
            }
        }

        public static void Clear() {
            count = 0;
        }

        public static void Add(int value) {
            q[count] = value;
            SiftUp(count);
            count++;
        }

        public static int Extract() {
            int top = q[0];
            q[0] = q[count - 1];
            count--;
            SiftDown(0);
            return top;
        }
    }
}

