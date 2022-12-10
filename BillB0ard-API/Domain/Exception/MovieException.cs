namespace BillB0ard_API.Domain.Exception
{
    public class MovieException : System.Exception
    {
        public MovieException() { }
        public MovieException(string message) : base(message) { }
    }

    public class MovieNotFoundException : MovieException
    {
        public MovieNotFoundException(string title) : base($"{title} cannot be found. Please check the title and retry.") { }

        public MovieNotFoundException(int id) : base($"There's no movie with the id : {id}. Please check the given id and retry.") { }

    }

    public class MovieAlreadyExistException : MovieException
    {
        public MovieAlreadyExistException(string title) : base($"{title} is already in the list.") { }
    }
}
