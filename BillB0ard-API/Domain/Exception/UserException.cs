using System.Runtime.Serialization;

namespace BillB0ard_API.Domain.Exception
{
    [Serializable]
    public class UserException : System.Exception
    {
        public UserException(string message) : base(message) { }

        protected UserException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }

    [Serializable]
    public class UserAlreadyExistException : UserException
    {
        public UserAlreadyExistException() : base($"The given user already exist, please change id or name and try again") { }

        public UserAlreadyExistException(string name) : base($"The user {name} already exist, please verify the name and try again") { }

        protected UserAlreadyExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }


}
