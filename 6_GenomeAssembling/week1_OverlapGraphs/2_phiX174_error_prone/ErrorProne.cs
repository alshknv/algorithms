using System;

namespace _2_phiX174_error_prone
{
    public static class ErrorProne
    {
        private static int[] Overlaps(string mer1, string mer2)
        {
            var result = new int[2] { -1, -1 };
            for (int i = 0; i < mer1.Length; i++)
            {
                if (result[0] < 0 && mer1[mer1.Length - i - 1] != mer2[i]) result[0] = i;
                if (result[1] < 0 && mer2[mer2.Length - i - 1] != mer1[i]) result[1] = i;
                if (result[0] >= 0 && result[1] >= 0) break;
            }
            return result;
        }

        public static string Assemble(string[] reads)
        {
            for (int i = 0; i < reads.Length; i++)
            {
                for (int j = i + 1; j < reads.Length; j++)
                {
                    var overlaps = Overlaps(reads[i], reads[j]);
                }
            }
            return "";
        }

        static void Main(string[] args)
        {
            var reads = new string[1618];
            for (int i = 0; i < 1618; i++)
                reads[i] = Console.ReadLine();
            Console.WriteLine(Assemble(reads));
        }
    }
}
