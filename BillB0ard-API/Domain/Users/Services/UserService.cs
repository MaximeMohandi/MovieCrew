using BillB0ard_API.Domain.Users.Dtos;
using BillB0ard_API.Domain.Users.Entities;
using BillB0ard_API.Domain.Users.Repository;

namespace BillB0ard_API.Domain.Users.Services
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
