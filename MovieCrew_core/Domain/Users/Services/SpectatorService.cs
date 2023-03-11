using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Repository;

namespace MovieCrew.Core.Domain.Users.Services
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
