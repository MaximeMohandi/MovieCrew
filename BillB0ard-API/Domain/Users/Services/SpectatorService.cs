using BillB0ard_API.Domain.Users.Entities;
using BillB0ard_API.Domain.Users.Repository;

namespace BillB0ard_API.Domain.Users.Services
{
    public class SpectatorService
    {
        private readonly UserRepository _movieRepository;

        public SpectatorService(UserRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<SpectatorDetailsEntity> FetchSpectator(long id)
        {
            return await _movieRepository.GetSpectatorDetails(id);
        }
    }
}
