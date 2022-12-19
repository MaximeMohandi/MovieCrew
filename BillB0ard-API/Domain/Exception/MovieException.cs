using System.Runtime.Serialization;

namespace BillB0ard_API.Domain.Exception
{
    [Serializable]
    public class MovieException : System.Exception
    {
        public MovieException(string message) : base(message) { }

        protected MovieException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }

    [Serializable]
    public class MovieNotFoundException : MovieException
    {
        public MovieNotFoundException(string title) : base($"{title} cannot be found. Please check the title and retry.") { }

        public MovieNotFoundException(int id) : base($"There's no movie with the id : {id}. Please check the given id and retry.") { }

        protected MovieNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }

    [Serializable]
    public class MovieAlreadyExistException : MovieException
    {
        public MovieAlreadyExistException(string title) : base($"{title} is already in the list.") { }

        protected MovieAlreadyExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }

    [Serializable]
    public class AllMoviesHaveBeenSeenException : MovieException
    {
        public AllMoviesHaveBeenSeenException() : base("It seems that you have seen all the movies in the list. Please try to add new one") { }

        protected AllMoviesHaveBeenSeenException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }

    [Serializable]
    public class MoviePosterFormatException : MovieException
    {
        public MoviePosterFormatException() : base("Poster must be a valid link. Please check the link and retry.") { }

        protected MoviePosterFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
