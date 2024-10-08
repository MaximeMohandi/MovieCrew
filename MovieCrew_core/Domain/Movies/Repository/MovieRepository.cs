﻿using Microsoft.EntityFrameworkCore;
using MovieCrew.Core.Data;
using MovieCrew.Core.Data.Models;
using MovieCrew.Core.Domain.Movies.Entities;
using MovieCrew.Core.Domain.Movies.Exception;
using MovieCrew.Core.Domain.Movies.Extension;
using MovieCrew.Core.Domain.Users.Exception;

namespace MovieCrew.Core.Domain.Movies.Repository;

public class MovieRepository : IMovieRepository
{
    private readonly AppDbContext _dbContext;

    public MovieRepository(AppDbContext databaseContext)
    {
        _dbContext = databaseContext;
    }

    public async Task<MovieDetailsEntity> GetMovie(string title)
    {
        var movie = await FetchMoviesWithNavigationProperties()
            .Where(m => m.Name.ToLower() == title.ToLower())
            .FirstOrDefaultAsync();
        return movie is null
            ? throw new MovieNotFoundException(title)
            : movie.ToDetailledEntity();
    }

    public async Task<MovieDetailsEntity> GetMovie(int id)
    {
        var movie = await FetchMoviesWithNavigationProperties()
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();

        return movie is null
            ? throw new MovieNotFoundException(id)
            : movie.ToDetailledEntity();
    }

    public async Task<List<MovieEntity>> GetAll()
    {
        var movies = await FetchMoviesWithNavigationProperties()
            .Select(m => m.ToEntity())
            .ToListAsync();

        if (movies.Count == 0) throw new NoMoviesFoundException();

        return movies;
    }

    public async Task<MovieEntity> Add(string title, string poster, string description, long? proposedById)
    {
        if (TitleExist(title))
            throw new MovieAlreadyExistException(title);

        if (proposedById != null && !_dbContext.Users.Any(u => u.Id == proposedById))
            throw new UserNotFoundException(proposedById.Value);

        var movie = new Movie
        {
            Name = title,
            Poster = poster,
            Description = description,
            DateAdded = DateTime.Now,
            ProposedById = proposedById
        };

        _dbContext.Movies.Add(movie);

        await _dbContext.SaveChangesAsync();

        return movie.ToEntity();
    }

    public async Task<List<MovieEntity>> GetAllUnSeen()
    {
        return await _dbContext.Movies
            .Where(m => !m.SeenDate.HasValue)
            .Select(m => m.ToEntity())
            .ToListAsync();
    }

    public async Task UpdateTitle(int movieId, string newTitle)
    {
        if (TitleExist(newTitle)) throw new MovieAlreadyExistException(newTitle);

        var movieToRename = FetchMovie(movieId);
        movieToRename.Name = newTitle;
        _dbContext.Movies.Update(movieToRename);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatePoster(int movieId, string newPoster)
    {
        var movieToRename = FetchMovie(movieId);
        movieToRename.Poster = newPoster;
        _dbContext.Movies.Update(movieToRename);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateDescription(int movieId, string metadataDescription)
    {
        var movieToChange = FetchMovie(movieId);
        movieToChange.Description = metadataDescription;
        _dbContext.Movies.Update(movieToChange);
        await _dbContext.SaveChangesAsync();
    }

    private IQueryable<Movie> FetchMoviesWithNavigationProperties()
    {
        return _dbContext.Movies
            .Include(m => m.Rates)
            .ThenInclude(r => r.User)
            .Include(m => m.ProposedBy);
    }

    private bool TitleExist(string title)
    {
        return _dbContext.Movies.Any(m => m.Name.ToLower() == title.ToLower());
    }

    private Movie FetchMovie(int id)
    {
        return _dbContext.Movies.FirstOrDefault(m => m.Id == id) ?? throw new MovieNotFoundException(id);
    }
}
