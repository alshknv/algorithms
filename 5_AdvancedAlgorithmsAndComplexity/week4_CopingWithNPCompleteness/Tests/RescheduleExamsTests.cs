using _4_reschedule_exams;
using Xunit;

namespace Tests
{
    public class RescheduleExamsTests
    {
        [Fact]
        public void Possible()
        {
            Assert.Equal("GGBR", RescheduleExams.Solve(new string[] {
                "4 5",
                "RRRG",
                "1 3",
                "1 4",
                "3 4",
                "2 4",
                "2 3"
            }));
        }

        [Fact]
        public void Impossible()
        {
            Assert.Equal("Impossible", RescheduleExams.Solve(new string[] {
                "4 5",
                "RGRR",
                "1 3",
                "1 4",
                "3 4",
                "2 4",
                "2 3"
            }));
        }
    }
}