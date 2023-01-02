using BillB0ard_API.Domain.DTOs;
using BillB0ard_API.Domain.Entities;
using BillB0ard_API.Domain.Repository;

namespace BillB0ard_API.Services
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

        public async Task<UserEntity> GetByID(int id)
        {
            return await _userRepository.GetBy(id);
        }

        public async Task<UserEntity> GetByName(string userName)
        {
            return await _userRepository.GetBy(userName);
        }
    }
}
