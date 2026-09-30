using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class TvSeries : Media, MediaInfo
{
    public SeasonDetails? SeasonAndEpisode { get; private set; } = default!;
    public string? Network { get; private set; }
    public ETvSeriesStatus Status { get; private set; }

    private TvSeries() { }

    public static TvSeries Create(
        string title, string desc, string lang, ReleaseDate? date, Guid genre,
        SeasonDetails seasonAndEpisode, string? network, ETvSeriesStatus status, Guid? id = null)
    {

        var series = new TvSeries
        {
            Id = id ?? Guid.NewGuid(),

        };
        series.SetBaseDetails(title, desc, lang, date, genre);
        series.SeasonAndEpisode = seasonAndEpisode;
        series.Network = network;
        series.Status = status;
        return series;
    }

    public void Update(
        string title, string desc, string lang, ReleaseDate? date, Guid genre,
        SeasonDetails seasonAndEpisode, string? network, ETvSeriesStatus status)
    {
        if (Status == ETvSeriesStatus.Canceled)
            throw new DomainException("Cannot update details of a canceled TV series.");
        if (Status == ETvSeriesStatus.Ended)
            throw new DomainException("Cannot change status of an already ended TV series.");
        SeasonAndEpisode = seasonAndEpisode;
        Network = network;
        Status = status;

        SetBaseDetails(title, desc, lang, date, genre);
    }

    public void UpdateSeasonAndEpisode(SeasonDetails seasonAndEpisode) => SeasonAndEpisode = seasonAndEpisode ?? throw new DomainException(nameof(seasonAndEpisode));
    public void UpdateNetwork(string? network) => Network = network;

    public void UpdateStatus(ETvSeriesStatus status) {
        if (Status == ETvSeriesStatus.Canceled)
            throw new DomainException("Cannot change status of a canceled TV series.");

        if (Status == ETvSeriesStatus.Ended)
            throw new DomainException("Cannot change status of an already ended TV series.");
        Status = status; 
    }

}