using System.IO;
using System.Linq;
using System;
using Xunit;
using _1_make_heap;

namespace Tests
{
    public class MakeHeapTests
    {
        private void Swap(long[] array, int a, int b)
        {
            var buf = array[a];
            array[a] = array[b];
            array[b] = buf;
        }

        private bool IsMinHeap(long[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                var child1 = i * 2 + 1;
                var child2 = i * 2 + 2;
                if ((child1 < array.Length && array[child1] < array[i]) ||
                    (child2 < array.Length && array[child2] < array[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private void CheckResult(string input, string[] result)
        {
            var array = input.Split(' ').Select(x => long.Parse(x)).ToArray();
            var swaps = int.Parse(result[0]);
            Assert.True(swaps <= array.Length * 4);
            for (int i = 1; i < result.Length; i++)
            {
                var sw = result[i].Split(' ');
                Swap(array, int.Parse(sw[0]), int.Parse(sw[1]));
            }
            Assert.True(IsMinHeap(array));
        }

        [Fact]
        public void Example1()
        {
            const string input = "5 4 3 2 1";
            CheckResult(input, MakeHeap.Solve(input));
        }

        [Fact]
        public void Example2()
        {
            Assert.Collection(MakeHeap.Solve("1 2 3 4 5"),
                l1 => Assert.Equal("0", l1));
            Assert.Collection(MakeHeap.Solve("0 1 5 2"),
                l1 => Assert.Equal("0", l1));
        }

        [Fact]
        public void Example3()
        {
            const string input = "10 9 8 7 6 5 4 3 2 1";
            CheckResult(input, MakeHeap.Solve(input));
        }

        [Fact]
        public void FromFile()
        {
            var input = File.ReadAllLines("../../../tests1/04").Last();
            CheckResult(input, MakeHeap.Solve(input));
        }
    }
}
