using System.Runtime.Serialization;

namespace BillB0ard_API.Domain.Exception
{
    [Serializable]
    public class RateException : System.Exception, ISerializable
    {
        public RateException(string message) : base(message) { }

        protected RateException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }

    [Serializable]
    public class RateLimitException : RateException
    {
        public RateLimitException(decimal rate) : base($"The rate must be between 0 and 10. Actual : {rate}") { }
    }
}
