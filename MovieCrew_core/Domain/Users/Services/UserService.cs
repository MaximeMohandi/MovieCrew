using MovieCrew.Core.Domain.Users.Dtos;
using MovieCrew.Core.Domain.Users.Entities;
using MovieCrew.Core.Domain.Users.Repository;

namespace MovieCrew.Core.Domain.Users.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUser(UserCreationDto userCreation)
        {
            await _userRepository.Add(userCreation);
        }

        public async Task<UserEntity> GetById(long id)
        {
            return await _userRepository.GetBy(id);
        }
    }
}
