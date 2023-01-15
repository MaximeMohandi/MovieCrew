using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;

namespace BillB0ard_API.Test.DataEqualityTest
{
    public class RateEqualityTest
    {
        [Test]
        public void SameRate()
        {
            Rate expectedRate = new()
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

        [Test]
        public void RateCompareToNull()
        {
            var actualRate = new Rate
            {
                Movie = new Movie(),
                MovieId = 1,
                Note = 2.0M,
                User = new User(),
                UserId = 1,

            };

            Assert.That(actualRate, Is.Not.EqualTo(null));
        }

        [Test]
        public void NotEqualToOtherType()
        {
            Rate actualRate = new()
            {
                Movie = new Movie(),
                MovieId = 1,
                Note = 2.0M,
                User = new User(),
                UserId = 1,

            };

            object otherObject = new() { };

            Assert.That(actualRate, Is.Not.EqualTo(otherObject));
        }

        [Test]
        public void RateEntityAreEquals()
        {
            RateEntity firstRate = new(new MovieEntity(1, "test", "", new DateTime(2023, 1, 3), new DateTime(2023, 1, 3)), new(1, "test", Domain.Enums.UserRoles.Admin), 2M);
            RateEntity secondRate = new(new MovieEntity(1, "test", "", new DateTime(2023, 1, 3), new DateTime(2023, 1, 3)), new(1, "test", Domain.Enums.UserRoles.Admin), 2M);

            Assert.That(firstRate, Is.EqualTo(secondRate));
        }

        [Test]
        public void ListOfRateEntityAreEqual()
        {
            List<RateEntity> firstList = new()
            {
                new(new MovieEntity(1, "test", "", new DateTime(2023, 1, 3), new DateTime(2023, 1, 3)), new(1, "test", Domain.Enums.UserRoles.Admin), 2M)
            };
            List<RateEntity> secondList = new()
            {
                new(new MovieEntity(1, "test", "", new DateTime(2023, 1, 3), new DateTime(2023, 1, 3)), new(1, "test", Domain.Enums.UserRoles.Admin), 2M)
            };

            Assert.That(firstList, Is.EqualTo(secondList));
        }

    }
}
