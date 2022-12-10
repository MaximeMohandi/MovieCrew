using BillB0ard_API.Data;
using BillB0ard_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BillB0ard_API.Test
{
    public class Tests
    {
        private static DbContextOptions<AppDbContext> _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MovieDbTest")
            .Options;

        AppDbContext dbContext;

        [OneTimeSetUp]
        public void Setup()
        {
            dbContext = new AppDbContext(_dbContextOptions);
            dbContext.Database.EnsureCreated();

            Movie[] movies = new[]
            {
                new Movie()
                {
                    Id = 1,
                    DateAdded = new DateTime(2022, 5, 10),
                    Name = "Lord of the ring",
                    Poster = "fakeLink",
                    SeenDate = new DateTime(2022, 5, 12)
                },
                new Movie()
                {
                    Id = 2,
                    DateAdded = new DateTime(2015, 8, 3),
                    Name = "Harry Potter",
                    Poster = null,
                    SeenDate = null
                },
                new Movie()
                {
                    Id = 3,
                    DateAdded = new DateTime(1996, 9, 21),
                    Name = "Jurassic Park",
                    Poster = "fakeLink",
                    SeenDate = new DateTime(1996, 9, 23),
                },
                new Movie()
                {
                    Id = 4,
                    DateAdded = new DateTime(2022, 10, 15),
                    Name = "Lord of the ring",
                    Poster = "fakeLink",
                    SeenDate = null
                },

            };

            dbContext.Movies.AddRange(movies);
            dbContext.SaveChanges();

        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            dbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task WhenIFetchAMovieWithItsTitle_ThenIGetTheMovie()
        {
            Movie fetchedMovies = dbContext.Movies.Where(m => m.Name == "Lord of the ring").First();
            Assert.That(fetchedMovies.Id, Is.EqualTo(1));
        }
    }
}