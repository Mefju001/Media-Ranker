using Domain.Exceptions;
using Domain.Value_Object;
using static System.Net.WebRequestMethods;

namespace Domain.Entity
{
    public abstract class Media
    {
        public int Id { get; init; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int GenreId { get; private set; }
        public ReleaseDate? ReleaseDate { get; private set; }
        public Language Language { get; private set; }
        public MediaStats Stats { get; private set; }
        private readonly List<Review> _reviews = new();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
        protected Media() { }
        protected Media(string Title, string Description, Language Language, ReleaseDate ReleaseDate, int genreDomain)
        {
            Validate(Title, Description);
            this.Title = Title;
            this.Description = Description;
            this.Language = Language;
            this.ReleaseDate = ReleaseDate;
            this.GenreId = genreDomain;
            this.Stats = new MediaStats(0,0);
        }
        protected Media(int id,string Title, string Description, Language Language, ReleaseDate ReleaseDate, int genreDomain, MediaStats stats)
        {
            Validate(Title, Description);
            this.Id = id;
            this.Title = Title;
            this.Description = Description;
            this.Language = Language;
            this.ReleaseDate = ReleaseDate;
            this.GenreId = genreDomain;
            this.Stats = stats;
        }
        protected void Update(string Title, string Description, Language Language, ReleaseDate ReleaseDate, int genreDomain)
        {
            Validate(Title, Description);
            this.Title = Title;
            this.Description = Description;
            this.Language = Language;
            this.ReleaseDate = ReleaseDate;
            this.GenreId = genreDomain;
        }
        public static void Validate(string Title, string Description)
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new ArgumentException("Title cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("Description cannot be null or empty.");
        }
        public void UpdateDetails(string title, string description, Language language)
        {
            Validate(title, description);
            Title = title;
            Description = description;
            Language = language;
        }
        public void AddReview(Review review)
        {
            if (Reviews.Any(r => r.UserId == review.UserId))
                throw new DomainException("User already reviewed this media.");
            _reviews.Add(review);
            var reviewCount = Reviews.Count;
            var avg = Reviews.Average(r => r.Rating.value);
            this.UpdateStatistic(reviewCount,avg);
        }
        public void UpdateStatistic(int reviewCount, double avg)
        {
            this.Stats = new MediaStats(avg,reviewCount);
        }
    }
}
