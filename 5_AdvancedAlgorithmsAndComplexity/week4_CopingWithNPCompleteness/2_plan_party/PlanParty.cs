using System.Linq;
using System.Collections.Generic;
using System;

namespace _2_plan_party
{
    public class Person
    {
        public List<int> Linked = new List<int>();
        public bool Visited;
        public int Fun;
    }

    public static class PlanParty
    {
        private static Person[] employees;
        public static string Solve(string[] input)
        {
            // init tree
            var n = int.Parse(input[0]);
            employees = new Person[n + 1];
            var fun = input[1].Split(' ').Select(x => int.Parse(x)).ToArray();
            var d = new int[n + 1]; //dp

            for (int i = 1; i <= n; i++)
            {
                employees[i] = new Person() { Fun = fun[i - 1] };
                d[i] = int.MaxValue;
            }
            for (int i = 2; i < input.Length; i++)
            {
                var uv = input[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                // add connection to both employees
                employees[uv[0]].Linked.Add(uv[1]);
                employees[uv[1]].Linked.Add(uv[0]);
            }
            var stack = new Stack<int>();
            var root = 0;
            for (int i = 1; i < employees.Length; i++)
            {
                // arbitrarily select root that has at most one connection
                if (employees[i].Linked.Count <= 1)
                {
                    stack.Push(i);
                    root = i;
                    break;
                }
            }

            // search
            while (stack.Count > 0)
            {
                var k = stack.Peek();
                if (employees[k].Visited)
                {
                    // if already was visited
                    k = stack.Pop();
                    if (d[k] == int.MaxValue)
                    {
                        if (employees[k].Linked.Count == 0)
                        {
                            // if no subordinates, save own fun factor
                            d[k] = employees[k].Fun;
                        }
                        else
                        {
                            //fun of current employee + fun of sub-subordinates
                            var fun1 = employees[k].Fun;
                            foreach (var sub in employees[k].Linked)
                            {
                                foreach (var subsub in employees[sub].Linked)
                                {
                                    fun1 += d[subsub];
                                }
                            }
                            // fun of subordinates
                            var fun0 = 0;
                            foreach (var sub in employees[k].Linked)
                            {
                                fun0 += d[sub];
                            }
                            // save maximum fun factor
                            d[k] = Math.Max(fun1, fun0);
                        }
                    }
                }
                else
                {
                    // visit, remove backward connections, add downward connections to stack
                    employees[k].Visited = true;
                    employees[k].Linked.RemoveAll(l => employees[l].Visited);
                    foreach (var l in employees[k].Linked)
                        stack.Push(l);
                }
            }

            return d[root].ToString();
        }
        static void Main(string[] args)
        {
            var nline = Console.ReadLine();
            var n = int.Parse(nline);
            var input = new string[n + 1];
            input[0] = nline;
            for (int i = 1; i <= n; i++)
            {
                input[i] = Console.ReadLine();
            }
            Console.WriteLine(Solve(input));
        }
    }
}
