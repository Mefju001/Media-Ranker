using Domain.Enums;
using Domain.Value_Object;

namespace Domain.Aggregate;

public class TvSeries : Media
{
    public int Seasons { get; private set; }
    public int Episodes { get; private set; }
    public string? Network { get; private set; }
    public EStatus Status { get; private set; }

    private TvSeries() { }

    public static TvSeries Create(
        string title, string desc, Language lang, ReleaseDate? date, int genre,
        int seasons, int episodes, string? network, EStatus status, int id = 0)
    {
        Validate(seasons, episodes);

        var series = new TvSeries
        {
            Id = id,
            Seasons = seasons,
            Episodes = episodes,
            Network = network,
            Status = status
        };

        series.SetBaseDetails(title, desc, lang, date, genre);
        return series;
    }

    public void Update(
        string title, string desc, Language lang, ReleaseDate? date, int genre,
        int seasons, int episodes, string? network, EStatus status)
    {
        Validate(seasons, episodes);
        Seasons = seasons;
        Episodes = episodes;
        Network = network;
        Status = status;

        SetBaseDetails(title, desc, lang, date, genre);
    }

    public void UpdateSeasons(int seasons)
    {
        if (seasons <= 0) throw new ArgumentException("Seasons must be > 0");
        Seasons = seasons;
    }

    public void UpdateEpisodes(int episodes)
    {
        if (episodes <= 0) throw new ArgumentException("Episodes must be > 0");
        Episodes = episodes;
    }

    public void UpdateNetwork(string? network) => Network = network;
    public void UpdateStatus(EStatus status) => Status = status;

    private static void Validate(int seasons, int episodes)
    {
        if (seasons <= 0 || episodes <= 0)
            throw new ArgumentException("Seasons and Episodes must be greater than zero.");
    }
}