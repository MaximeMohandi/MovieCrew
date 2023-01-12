using BillB0ard_API.Domain.Enums;

namespace BillB0ard_API.Domain.Entities
{
    public class UserEntity
    {
        public UserEntity(long id, string name, UserRoles role = UserRoles.None)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public long Id { get; }
        public string Name { get; }
        public UserRoles Role { get; }
    }
}