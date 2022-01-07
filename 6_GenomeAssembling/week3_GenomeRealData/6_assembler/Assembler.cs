using System.Collections.Generic;
using System;

namespace _6_assembler
{
    public static class Assembler
    {
        public static string[] Assemble(string[] data)
        {
            throw new NotImplementedException();
        }

        static void Main(string[] args)
        {
            var input = new List<string>();
            string inLine;
            while ((inLine = Console.ReadLine()) != null)
            {
                input.Add(inLine);
            }
            Console.WriteLine(Assemble(input.ToArray()));
        }
    }
}
