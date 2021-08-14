using Xunit;
using _2_primitive_calculator;

namespace Tests
{
    public class PrimitiveCalculatorTests
    {
        [Fact]
        public void Simple()
        {
            Assert.Collection(PrimitiveCalculator.Solve("1"),
                l1 => Assert.Equal("0", l1),
                l2 => Assert.Equal("1", l2));
            Assert.Collection(PrimitiveCalculator.Solve("5"),
                l1 => Assert.Equal("3", l1),
                l2 => Assert.Equal("1 3 4 5", l2));
        }

        [Fact]
        public void Longer()
        {
            Assert.Collection(PrimitiveCalculator.Solve("96234"),
                l1 => Assert.Equal("14", l1),
                l2 => Assert.Equal("1 3 9 10 11 33 99 297 891 2673 8019 16038 16039 48117 96234", l2));
        }
    }
}