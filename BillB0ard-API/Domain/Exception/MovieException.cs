namespace BillB0ard_API.Domain.Exception
{
    public class MovieException : System.Exception
    {
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

    public class AllMoviesHaveBeenSeenException : MovieException
    {
        public AllMoviesHaveBeenSeenException() : base("It seems that you have seen all the movies in the list. Please try to add new one") { }
    }

    public class MoviePosterFormatException : MovieException
    {
        public MoviePosterFormatException() : base("Poster must be a valid link. Please check the link and retry.") { }
    }

    public class NoMoviesFoundException : MovieException
    {
        public NoMoviesFoundException() : base("It seem that there's no movies in the list. Please try to add new one") { }
    }
}
