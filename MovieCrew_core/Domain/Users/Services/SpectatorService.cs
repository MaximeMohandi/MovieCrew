using MovieCrew_core.Domain.Users.Entities;
using MovieCrew_core.Domain.Users.Repository;

namespace MovieCrew_core.Domain.Users.Services
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
