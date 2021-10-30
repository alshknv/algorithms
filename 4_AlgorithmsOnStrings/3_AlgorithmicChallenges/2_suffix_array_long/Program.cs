using System.Collections.Generic;
using System.Net.Mime;
using System;
using System.Linq;

namespace _2_suffix_array_long
{
    public static class SuffixArrayLong
    {
        private static int[] SortChars(string text)
        {
            var order = new int[text.Length];
            var count = new SortedDictionary<char, int>();
            for (int i = 0; i < text.Length; i++)
                count[text[i]] = (count.ContainsKey(text[i]) ? count[text[i]] : 0) + 1;
            var keys = count.Keys.ToArray();
            for (int j = 1; j < keys.Length; j++)
                count[keys[j]] += count[keys[j - 1]];
            for (int k = text.Length - 1; k >= 0; k--)
            {
                var ch = text[k];
                count[ch]--;
                order[count[ch]] = k;
            }
            return order;
        }

        private static int[] ComputeClasses(string text, int[] order)
        {
            var classArr = new int[text.Length];
            classArr[order[0]] = 0;
            for (int i = 1; i < text.Length; i++)
            {
                classArr[order[i]] = classArr[order[i - 1]] + (text[order[i]] != text[order[i - 1]] ? 1 : 0);
            }
            return classArr;
        }

        private static int[] Sort2L(int textlen, int l, int[] order, int[] classArr)
        {
            var count = new int[textlen];
            var newOrder = new int[textlen];
            for (int i = 0; i < textlen; i++)
                count[classArr[i]]++;
            for (int j = 1; j < textlen; j++)
                count[j] += count[j - 1];
            for (int k = textlen - 1; k >= 0; k--)
            {
                var start = (order[k] - l + textlen) % textlen;
                var cl = classArr[start];
                count[cl]--;
                newOrder[count[cl]] = start;
            }
            return newOrder;
        }

        private static int[] UpdateClasses(int[] newOrder, int[] classArr, int l)
        {
            var newClass = new int[newOrder.Length];
            newClass[newOrder[0]] = 0;
            for (int i = 1; i < newOrder.Length; i++)
            {
                var current = newOrder[i];
                var previous = newOrder[i - 1];
                var middle = (current + l) % newOrder.Length;
                var middlePrev = (previous + l) % newOrder.Length;
                newClass[current] = newClass[previous] + (classArr[current] != classArr[previous] || classArr[middle] != classArr[middlePrev] ? 1 : 0);
            }
            return newClass;
        }

        public static string Solve(string text)
        {
            var order = SortChars(text);
            var classArr = ComputeClasses(text, order);
            var textlen = text.Length;
            var l = 1;
            while (l < textlen)
            {
                order = Sort2L(textlen, l, order, classArr);
                classArr = UpdateClasses(order, classArr, l);
                l = 2 * l;
            }
            return string.Join(" ", order);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Solve(Console.ReadLine()));
        }
    }
}
