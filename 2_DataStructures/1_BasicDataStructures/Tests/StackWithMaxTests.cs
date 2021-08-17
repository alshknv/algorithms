using Xunit;
using _4_stack_with_max;

namespace Tests
{
    public class StackWithMaxTests
    {
        [Fact]
        public void Examples()
        {
            Assert.Collection(StackWithMax.Solve(new string[5] {
                "push 2","push 1","max","pop","max" }),
                l1 => Assert.Equal("2", l1),
                l2 => Assert.Equal("2", l2));
            Assert.Collection(StackWithMax.Solve(new string[5] {
                "push 1","push 2","max","pop","max" }),
                l1 => Assert.Equal("2", l1),
                l2 => Assert.Equal("1", l2));
            Assert.Collection(StackWithMax.Solve(new string[10] {
                "push 2","push 3","push 9","push 7","push 2","max","max","max","pop","max" }),
                l1 => Assert.Equal("9", l1),
                l2 => Assert.Equal("9", l2),
                l3 => Assert.Equal("9", l3),
                l4 => Assert.Equal("9", l4));
            Assert.Empty(StackWithMax.Solve(new string[3] {
                "push 1","push 7","pop" }));
            Assert.Collection(StackWithMax.Solve(new string[6] {
                "push 7","push 1","push 7","max","pop","max" }),
                l1 => Assert.Equal("7", l1),
                l2 => Assert.Equal("7", l2));
        }

        [Fact]
        public void Errors()
        {
            Assert.Collection(StackWithMax.Solve(new string[8] {
                "push 2","push 1","max","pop","max","pop","pop","max" }),
                l1 => Assert.Equal("2", l1),
                l2 => Assert.Equal("2", l2));
            Assert.Collection(StackWithMax.Solve(new string[3] {
                "pop","push 2","max" }),
                l1 => Assert.Equal("2", l1));
            Assert.Empty(StackWithMax.Solve(new string[1] {
                "pop" }));
        }
    }
}