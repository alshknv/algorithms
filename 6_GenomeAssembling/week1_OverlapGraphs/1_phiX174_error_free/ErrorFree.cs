using System.Collections.Generic;
using System;

namespace _1_phiX174_error_free
{
    public static class ErrorFree
    {
        public static string Assemble(string[] reads)
        {
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
