using System;
using Xunit;
using _3_k_universal_circular_string;

namespace Tests
{
    public class KUniversalCircularStringTests
    {
        [Fact]
        public void UniversalTest()
        {
            for (int k = 3; k < 15; k++)
            {
                var length = (int)Math.Pow(2, k);
                var universalString = new string('0', k - 1) + KUniversalCircularString.Solve(k);
                for (int i = 0; i < length; i++)
                {
                    var mer = Convert.ToString(i, 2).PadLeft(k, '0');
                    Assert.Contains(mer, universalString);
                }
            }
        }
    }
}