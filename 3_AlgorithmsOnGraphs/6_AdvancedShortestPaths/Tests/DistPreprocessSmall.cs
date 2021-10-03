using _3_dist_preprocess_small;
using Xunit;

namespace Tests
{
    public class DistPreprocessSmallTests
    {
        [Fact]
        public void Example1()
        {
            DistPreprocessSmall.Preprocess(6, new string[] {
                "2 6 2", "6 1 3","1 4 2","4 3 1","3 5 2"
            });
        }
    }
}