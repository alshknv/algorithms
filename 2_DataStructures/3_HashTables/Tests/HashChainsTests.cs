using System.IO;
using System;
using System.Linq;
using Xunit;
using _2_hash_chains;

namespace Tests
{
    public class HashChainsTests
    {
        [Fact]
        public void Example1()
        {
            var queries = new string[] {
                "add world",
                "add HellO",
                "check 4",
                "find World",
                "find world",
                "del world",
                "check 4",
                "del HellO",
                "add luck",
                "add GooD",
                "check 2",
                "del good"
            };
            Assert.Collection(HashChains.Solve(5, queries),
                l1 => Assert.Equal("HellO world", l1),
                l2 => Assert.Equal("no", l2),
                l3 => Assert.Equal("yes", l3),
                l4 => Assert.Equal("HellO", l4),
                l5 => Assert.Equal("GooD luck", l5));
        }

        [Fact]
        public void Example2()
        {
            var queries = new string[] {
                "add test",
                "add test",
                "find test",
                "del test",
                "find test",
                "find Test",
                "add Test",
                "find Test"
            };
            Assert.Collection(HashChains.Solve(4, queries),
                l1 => Assert.Equal("yes", l1),
                l2 => Assert.Equal("no", l2),
                l3 => Assert.Equal("no", l3),
                l4 => Assert.Equal("yes", l4));
        }

        [Fact]
        public void Example3()
        {
            var queries = new string[] {
                "check 0",
                "find help",
                "add help",
                "add del",
                "add add",
                "find add",
                "find del",
                "del del",
                "find del",
                "check 0",
                "check 1",
                "check 2"
            };
            Assert.Collection(HashChains.Solve(3, queries),
                l0 => Assert.Equal("", l0),
                l1 => Assert.Equal("no", l1),
                l2 => Assert.Equal("yes", l2),
                l3 => Assert.Equal("yes", l3),
                l4 => Assert.Equal("no", l4),
                l5 => Assert.Equal("", l5),
                l6 => Assert.Equal("add help", l6),
                l7 => Assert.Equal("", l7));
        }

        [Fact]
        public void FromFile()
        {
            var testfiles = Directory.EnumerateFiles("../../../tests2");
            foreach (var tf in testfiles)
            {
                if (!tf.Contains(".a"))
                {
                    var lines = File.ReadAllLines(tf);
                    var answer = File.ReadAllLines($"{tf}.a").Select(x => x.TrimEnd()).ToArray();
                    Assert.Equal(answer, HashChains.Solve(int.Parse(lines[0]), lines.Skip(2).ToArray()));
                }
            }
        }
    }
}
