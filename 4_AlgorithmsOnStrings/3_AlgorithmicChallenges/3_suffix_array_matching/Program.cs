using System.Linq;
using System.Collections.Generic;
using System;

namespace _3_suffix_array_matching
{
    public static class SuffixArrayMatching
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

        private static int[] BuildSuffixArray(string text)
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
            return order;
        }

        private static int CompareToSuffix(string text, string pattern, int suffix)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (i + suffix == text.Length) return 1;
                if (pattern[i] < text[suffix + i]) return -1;
                if (pattern[i] > text[suffix + i]) return 1;
            }
            return 0;
        }
        public static string Solve(string text, string patternString)
        {
            text += "$";
            var patterns = patternString.Split(' ');
            var suffixArray = BuildSuffixArray(text);
            var result = new HashSet<int>();
            for (int i = 0; i < patterns.Length; i++)
            {
                var min = 0;
                var max = text.Length;
                while (min < max)
                {
                    var mid = (min + max) / 2;
                    if (CompareToSuffix(text, patterns[i], suffixArray[mid]) > 0)
                        min = mid + 1;
                    else max = mid;
                }
                var start = min;
                max = text.Length;
                while (min < max)
                {
                    var mid = (min + max) / 2;
                    if (CompareToSuffix(text, patterns[i], suffixArray[mid]) < 0)
                        max = mid;
                    else min = mid + 1;
                }
                var end = max;

                for (int j = start; j < end; j++)
                {
                    result.Add(suffixArray[j]);
                }
            }
            return string.Join(" ", result);
        }

        static void Main(string[] args)
        {
            var text = Console.ReadLine();
            Console.ReadLine();
            var patterns = Console.ReadLine();
            Console.WriteLine(Solve(text, patterns));
        }
    }
}
