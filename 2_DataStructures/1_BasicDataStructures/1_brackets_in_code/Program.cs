using System;
using System.Collections.Generic;
using System.Linq;

namespace _1_brackets_in_code
{
    public static class BracketsInCode
    {
        private class Bracket
        {
            public char Symbol;
            public int Index;

            public Bracket(char symbol, int index)
            {
                Symbol = symbol;
                Index = index;
            }
        }

        public static string Solve(string input)
        {
            var bracketStack = new Stack<Bracket>();
            var openBrackets = new char[3] { '(', '[', '{' };
            var bracketMatch = new Dictionary<char, char>() { { ')', '(' }, { ']', '[' }, { '}', '{' } };
            for (int i = 0; i < input.Length; i++)
            {
                if (Array.IndexOf(openBrackets, input[i]) >= 0)
                {
                    bracketStack.Push(new Bracket(input[i], i));
                }
                else
                {
                    Bracket lastBracket;
                    if (bracketMatch.ContainsKey(input[i]))
                    {
                        if (bracketStack.TryPeek(out lastBracket) && lastBracket.Symbol == bracketMatch[input[i]])
                        {
                            bracketStack.Pop();
                        }
                        else
                        {
                            return (i + 1).ToString();
                        }
                    }
                }
            }
            if (bracketStack.Count > 0)
            {
                return (bracketStack.Pop().Index + 1).ToString();
            }
            return "Success";
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
