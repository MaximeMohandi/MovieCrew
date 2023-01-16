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

        // override object.Equals
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            UserEntity toCompare = (UserEntity)obj;
            return Id.Equals(toCompare.Id) && Name.Equals(toCompare.Name)
                && Role.Equals(toCompare.Role);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Name.GetHashCode();
        }
    }
}