using BillB0ard_API.Domain.Entities;

namespace BillB0ard_API.Domain.Repository
{
    public class MovieRepository
    {
        public MovieRepository()
        {

        }

        public async Task<Movie> GetMovie(string title)
        {
            return new(1, "Lord of the ring", "fakeLink", new DateTime(2022, 5, 10), new DateTime(2022, 5, 12));
        }
    }
}
