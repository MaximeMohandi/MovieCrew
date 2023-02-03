using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;

namespace BillB0ard_API.Test.DataEqualityTest
{
    public class RateEqualityTest
    {
        [Test]
        public void SameRateModel()
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

            Assert.Multiple(() =>
            {
                Assert.That(actualRate, Is.EqualTo(expectedRate));
                Assert.That(actualRate.GetHashCode(), Is.EqualTo(expectedRate.GetHashCode()));
            });
        }

        [Test]
        public void RateModelComparedToNull()
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
        public void RateEntityAreEquals()
        {
            RateEntity firstRate = new(new(1, "test", Domain.Enums.UserRoles.Admin), 2M);
            RateEntity secondRate = new(new(1, "test", Domain.Enums.UserRoles.Admin), 2M);

            Assert.Multiple(() =>
            {
                Assert.That(firstRate, Is.EqualTo(secondRate));
                Assert.That(firstRate.GetHashCode(), Is.EqualTo(secondRate.GetHashCode()));
            });
        }

        [Test]
        public void ListOfRateEntityAreEqual()
        {
            List<RateEntity> firstList = new()
            {
                new(new(1, "test", Domain.Enums.UserRoles.Admin), 2M)
            };
            List<RateEntity> secondList = new()
            {
                new(new(1, "test", Domain.Enums.UserRoles.Admin), 2M)
            };

            CollectionAssert.AreEqual(firstList, secondList);
        }

    }
}
