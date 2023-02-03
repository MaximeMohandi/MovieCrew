using BillB0ard_API.Domain.Users.Enums;

namespace BillB0ard_API.Domain.Users.Entities
{
    public class UserEntity
    {
        public UserEntity(long id, string name, UserRoles role = UserRoles.None)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public UserEntity(long id, string name, int role)
        {
            Id = id;
            Name = name;
            Role = (UserRoles)role;
        }

        public long Id { get; }
        public string Name { get; }
        public UserRoles Role { get; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            UserEntity toCompare = (UserEntity)obj;

            return Id == toCompare.Id
                && Name == toCompare.Name
                && Role == toCompare.Role;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Role);
        }
    }
}