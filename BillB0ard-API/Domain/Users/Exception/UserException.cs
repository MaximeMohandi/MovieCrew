namespace BillB0ard_API.Domain.Users.Exception
{
    public class UserException : System.Exception
    {
        public UserException(string message) : base(message) { }
    }

    public class UserAlreadyExistException : UserException
    {
        public UserAlreadyExistException(string name) : base($"The user {name} already exist. please verify the name and try again") { }
    }

    public class UserNotFoundException : UserException
    {
        public UserNotFoundException(long id) : base($"User with id: {id} not found. Please check the conformity and try again") { }

    }
}
