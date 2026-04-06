using Domain.Base;
using Domain.Entity;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Value_Object;

public abstract class Media : AggregateRoot<int>,IAudited
{
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int GenreId { get; private set; }
    public ReleaseDate? ReleaseDate { get; private set; }
    public Language Language { get; private set; } = default!;
    public MediaStats Stats { get; private set; } = new(0, 0);
    public AuditInfo AuditInfo { get; private set; } = new();

    private readonly List<Review> reviews = new();
    public IReadOnlyCollection<Review> Reviews => reviews.AsReadOnly();
    protected Media() { }


    protected void SetBaseDetails(string title, string description, Language language, ReleaseDate? releaseDate, int genreId)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Title and Description are required.");
        this.AuditInfo = AuditInfo.MarkAsUpdated();
        Title = title;
        Description = description;
        Language = language;
        ReleaseDate = releaseDate;
        GenreId = genreId;
    }

    public void AddReview(Review review)
    {
        if (reviews.Any(r => r.UserId == review.UserId))
            throw new DomainException("User already reviewed this media.");

        reviews.Add(review);
        RecalculateStats();
    }

    public void EditReview(int reviewId, Guid userId, Rating rating, string comment)
    {
        var review = reviews.FirstOrDefault(r => r.Id == reviewId) ?? throw new NotFoundException("Review not found.");
        if (review.UserId != userId) throw new DomainException("Unauthorized edit.");

        review.Update(rating, comment);
        RecalculateStats();
    }

    public void DeleteReview(int reviewId)
    {
        var review = reviews.FirstOrDefault(r => r.Id == reviewId) ?? throw new DomainException("Review not found.");
        reviews.Remove(review);
        RecalculateStats();
    }
    private void RecalculateStats() =>
        Stats = new MediaStats(reviews.Any() ? reviews.Average(r => r.Rating) : 0, reviews.Count);

}