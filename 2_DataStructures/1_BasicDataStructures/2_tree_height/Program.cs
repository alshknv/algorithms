using System.Collections.Generic;
using System.Linq;
using System;

namespace _2_tree_height
{
    public class Node
    {
        public int Level;
        public List<Node> Children = new List<Node>();
    }

    public class QueueItem<T>
    {
        public QueueItem<T> Next;
        public QueueItem<T> Prev;
        public T Item;
    }

    public class MyQueue<T>
    {
        private QueueItem<T> begin;
        private QueueItem<T> end;

        public void Enqueue(T item)
        {
            var newEnd = new QueueItem<T>()
            {
                Item = item
            };
            if (end == null)
            {
                end = begin = newEnd;
            }
            else
            {
                newEnd.Prev = new QueueItem<T>()
                {
                    Item = end.Item,
                    Next = newEnd,
                    Prev = end
                };
                if (begin.Equals(end)) begin = newEnd.Prev;
                end.Next = newEnd;
                end = newEnd;
            }
        }

        public T Dequeue()
        {
            var item = begin.Item;
            begin = begin.Next;
            if (begin == null) end = null;
            return item;
        }

        public bool Empty()
        {
            return begin == null;
        }
    }

    public static class TreeHeight
    {
        public static string Solve(string input)
        {
            var data = input.Split(' ').Select(x => int.Parse(x)).ToArray();
            var nodes = data.Select(x => new Node()).ToArray();
            var queue = new MyQueue<Node>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == -1)
                {
                    nodes[i].Level = 1;
                    queue.Enqueue(nodes[i]);
                }
                else
                {
                    nodes[data[i]].Children.Add(nodes[i]);
                }
            }

            // breadth-first
            var height = 0;
            while (!queue.Empty())
            {
                var node = queue.Dequeue();
                if (node.Level > height)
                {
                    height = node.Level;
                }
                for (int i = 0; i < node.Children.Count; i++)
                {
                    node.Children[i].Level = node.Level + 1;
                    queue.Enqueue(node.Children[i]);
                }
            }
            return height.ToString();
        }

        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
