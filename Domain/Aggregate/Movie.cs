using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class Movie : Media, MediaInfo
{
    public Guid DirectorId { get; private set; }
    public Duration Duration { get; private set; } = default!;
    public EDistributionType DistributionType { get; private set; } = EDistributionType.DirectToVideo;
    public EMovieStatus Status { get; private set; } = EMovieStatus.Announced;

    private Movie() { }

    public static Movie Create(
        string title, string desc, string lang, ReleaseDate? date, Guid genre,
        Guid director, Duration duration, EDistributionType distributionType, EMovieStatus status, Guid? id = null)
    {
        if (director == Guid.Empty)
            throw new DomainException("Director identifier cannot be empty.");

        var movie = new Movie
        {
            Id = id ?? Guid.NewGuid(),
            
        };
        movie.SetBaseDetails(title, desc, lang, date, genre);
        movie.DirectorId = director;
        movie.Duration = duration ?? throw new DomainException(nameof(duration));
        movie.DistributionType = distributionType;
        movie.Status = status;
        return movie;
    }

    public void Update(
        string title, string desc, string lang, ReleaseDate? date, Guid genre,
        Guid director, Duration duration, EDistributionType distributionType, EMovieStatus status)
    {
        if (Status == EMovieStatus.Cancelled)
            throw new DomainException("Cannot update details of a canceled movie.");

        if (Status == EMovieStatus.Released)
            throw new DomainException("Cannot change status of an already released movie.");

        if (director == Guid.Empty)
            throw new DomainException("Director identifier cannot be empty.");
        SetBaseDetails(title, desc, lang, date, genre);
        DirectorId = director;
        Duration = duration ?? throw new DomainException(nameof(duration));
        DistributionType = distributionType;
        Status = status;
    }
        

    public void UpdateStatus(EMovieStatus status)
    {
        if (Status == EMovieStatus.Cancelled)
            throw new DomainException("Cannot change status of a canceled movie.");

        if (Status == EMovieStatus.Released)
            throw new DomainException("Cannot change status of an already released movie.");

        Status = status;
    }
}