using Domain.Entity;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Value_Object;

public abstract class Media : AggregateRoot<Guid>, IAudited
{
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public Guid GenreId { get; private set; }
    public ReleaseDate? ReleaseDate { get; private set; }
    public string Language { get; private set; } = default!;
    public MediaStats Stats { get; private set; } = new(0, 0);
    public AuditInfo AuditInfo { get; private set; } = new();

    private readonly List<Review> reviews = new();
    public IReadOnlyCollection<Review> Reviews => reviews.AsReadOnly();
    protected Media() { }


    protected void SetBaseDetails(string title, string description, string language, ReleaseDate? releaseDate, Guid genreId)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
            throw new DomainException("Title and Description are required.");

        if (string.IsNullOrWhiteSpace(language))
            throw new DomainException("Language is required.");

        if (genreId == Guid.Empty)
            throw new DomainException("Genre identifier cannot be empty.");

        if (!string.IsNullOrEmpty(Title))
        {
            AuditInfo = AuditInfo.MarkAsUpdated();
        }
        Title = title;
        Description = description;
        Language = language;
        ReleaseDate = releaseDate;
        GenreId = genreId;
    }
    public bool HasUserReviewed(Guid userId) => reviews.Any(r => r.UserId == userId);

    public void AddReview(Guid userId, Rating rating, string comment, string username)
    {
        if (reviews.Any(r => r.UserId == userId))
            throw new DomainException("User already reviewed this media.");
        var review = Review.Create(rating, comment, Id, userId, username);
        reviews.Add(review);
        ApplyReviewToStats(rating.Value);
    }

    public void EditReview(Guid reviewId, Guid userId, Rating rating, string comment)
    {
        var review = reviews.FirstOrDefault(r => r.Id == reviewId) ?? throw new DomainException("Review not found.");
        if (review.UserId != userId) throw new DomainException("Unauthorized edit.");
        double oldRatingValue = review.Rating.Value;
        review.Update(rating, comment);
        UpdateReviewInStats(oldRatingValue, rating.Value);
    }

    public void DeleteReview(Guid reviewId)
    {
        var review = reviews.FirstOrDefault(r => r.Id == reviewId) ?? throw new DomainException("Review not found.");
        double ratingValueToRemove = review.Rating.Value;
        reviews.Remove(review);
        RemoveReviewFromStats(ratingValueToRemove);
    }
    private void ApplyReviewToStats(double newRating)
    {
        int newCount = Stats.ReviewCount + 1;
        double newAverage = Stats.AverageRating + ((newRating - Stats.AverageRating) / newCount);
        Stats = new MediaStats(newAverage, newCount);
    }

    private void UpdateReviewInStats(double oldRating, double newRating)
    {
        if (Stats.ReviewCount == 0) return;
        double newAverage = Stats.AverageRating + ((newRating - oldRating) / Stats.ReviewCount);
        Stats = new MediaStats(newAverage, Stats.ReviewCount);
    }

    private void RemoveReviewFromStats(double removedRating)
    {
        int newCount = Stats.ReviewCount - 1;
        if (newCount <= 0)
        {
            Stats = new MediaStats(0, 0);
            return;
        }
        double newAverage = ((Stats.AverageRating * Stats.ReviewCount) - removedRating) / newCount;
        Stats = new MediaStats(newAverage, newCount);
    }

}