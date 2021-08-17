using System;
using System.Collections.Generic;
using System.Linq;

namespace _1_brackets_in_code
{
    public class Bracket
    {
        public char Symbol;
        public int Index;
        public Bracket Next;

        public static Bracket Create(Bracket bracket = null)
        {
            return bracket == null ? null :
            new Bracket(bracket.Symbol, bracket.Index, bracket.Next);
        }

        public Bracket(char symbol, int index, Bracket next = null)
        {
            Symbol = symbol;
            Index = index;
            Next = next;
        }
    }

    public class StackItem<T>
    {
        public StackItem<T> Next;
        public T Item;
    }

    public class MyStack<T>
    {
        private StackItem<T> top;
        public void Push(T item)
        {
            top = new StackItem<T>()
            {
                Item = item,
                Next = top
            };
        }

        public T Pop()
        {
            var pop = top.Item;
            top = top.Next;
            return pop;
        }

        public T Top()
        {
            return top.Item;
        }

        public bool Empty()
        {
            return top == null;
        }
    }

    public static class BracketsInCode
    {
        public static string Solve(string input)
        {
            var bracketStack = new MyStack<Bracket>();
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
                    if (bracketMatch.ContainsKey(input[i]))
                    {
                        if (!bracketStack.Empty() && bracketStack.Top().Symbol == bracketMatch[input[i]])
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
            if (!bracketStack.Empty())
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
