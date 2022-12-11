using BillB0ard_API.Data.Models;

namespace BillB0ard_API.Test.RateTest
{
    public class RateEqualityTest
    {
        [Test]
        public void SameRate()
        {
            Rate expectedRate = new Rate
            {
                Movie = new Movie(),
                MovieId = 1,
                Note = 2.0M,
                User = new User(),
                UserId = 1,

            };

            var actualRate = new Rate
            {
                Movie = new Movie(),
                MovieId = 1,
                Note = 2.0M,
                User = new User(),
                UserId = 1,

            };

            Assert.That(actualRate, Is.EqualTo(expectedRate));
        }
    }
}
