using System.Collections.Generic;
using System;

namespace _4_stack_with_max
{
    public class StackItem
    {
        public StackItem Next;
        public long Max;
        public long Value;
    }

    public class MaxStack
    {
        private StackItem top;
        public void Push(long value)
        {
            top = new StackItem()
            {
                Value = value,
                Max = Math.Max(top?.Max ?? long.MinValue, value),
                Next = top
            };
        }

        public long Pop()
        {
            var pop = top.Value;
            top = top.Next;
            return pop;
        }

        public long Max()
        {
            return top.Max;
        }

        public bool Empty()
        {
            return top == null;
        }
    }

    public static class StackWithMax
    {
        public static List<string> Solve(string[] queries)
        {
            var stack = new MaxStack();
            var result = new List<string>();
            foreach (var query in queries)
            {
                var parts = query.Split(' ');
                switch (parts[0])
                {
                    case "push":
                        stack.Push(long.Parse(parts[1]));
                        break;
                    case "pop":
                        if (!stack.Empty())
                            stack.Pop();
                        break;
                    case "max":
                        if (!stack.Empty())
                            result.Add(stack.Max().ToString());
                        break;
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var queries = new string[n];
            for (int i = 0; i < n; i++)
            {
                queries[i] = Console.ReadLine();
            }
            foreach (var line in Solve(queries))
            {
                Console.WriteLine(line);
            }
        }
    }
}
