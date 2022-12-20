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

        public async Task AddUser(string name)
        {
            await _userRepository.Add(new(name));
        }
    }
}
