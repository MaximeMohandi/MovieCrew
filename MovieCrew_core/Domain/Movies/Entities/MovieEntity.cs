﻿namespace MovieCrew.Core.Domain.Movies.Entities;

public class MovieEntity
{
    public MovieEntity(int id, string title, string poster, string description, DateTime addedDate, DateTime? seenDate,
        decimal? averageRate)
    {
        Id = id;
        Title = title;
        Poster = poster;
        Description = description;
        DateAdded = addedDate;
        ViewingDate = seenDate;
        AverageRate = averageRate;
    }

    public int Id { get; }
    public string Title { get; }
    public string Poster { get; }
    public string Description { get; }
    public DateTime DateAdded { get; }
    public DateTime? ViewingDate { get; }
    public decimal? AverageRate { get; }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        var toCompare = (MovieEntity)obj;


        return Id.Equals(toCompare.Id) && Title == toCompare.Title
                                       && Poster == toCompare.Poster && Description == toCompare.Description
                                       && DateAdded == toCompare.DateAdded
                                       && ViewingDate == toCompare.ViewingDate && AverageRate == toCompare.AverageRate;
    }

    public override int GetHashCode()
    {
        var hash = HashCode.Combine(Id, Title, Poster, Description, DateAdded);
        hash = HashCode.Combine(hash, ViewingDate is null ? 0 : ViewingDate.GetHashCode());
        hash = HashCode.Combine(hash, AverageRate is null ? 0 : AverageRate.GetHashCode());
        return hash;
    }
}