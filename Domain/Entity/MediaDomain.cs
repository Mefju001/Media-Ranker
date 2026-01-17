using Domain.Exceptions;

namespace Domain.Entity
{
    public abstract class MediaDomain
    {
        public int Id { get; init; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int GenreId { get; private set; }
        public DateTime ReleaseDate { get; private set; } = DateTime.UtcNow;
        public string? Language { get; private set; }
        public MediaStatsDomain Stats { get; private set; }
        public List<ReviewDomain> Reviews { get; private set; } = new List<ReviewDomain>();
        protected MediaDomain(string Title, string Description, string Language, DateTime ReleaseDate, int GenreId)
        {
            Validate(Title, Description, Language);
            this.Title = Title;
            this.Description = Description;
            this.Language = Language;
            this.ReleaseDate = ReleaseDate;
            this.GenreId = GenreId;
            this.Stats = MediaStatsDomain.Create();
        }
       
        protected void Update(string Title, string Description, string Language, DateTime ReleaseDate, int GenreId)
        {
            Validate(Title, Description, Language);
            this.Title = Title;
            this.Description = Description;
            this.Language = Language;
            this.ReleaseDate = ReleaseDate;
            this.GenreId = GenreId;
        }
        public static void Validate(string Title, string Description, string Language)
        {
            if (string.IsNullOrWhiteSpace(Title))
                throw new ArgumentException("Title cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("Description cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(Language))
                throw new ArgumentException("Language cannot be null or empty.");
        }
        public void UpdateDetails(string title, string description, string language)
        {
            Validate(title, description, language);
            Title = title;
            Description = description;
            Language = language;
        }
        public void AddReview(ReviewDomain review)
        {
            if (Reviews.Any(r => r.UserId == review.UserId))
                throw new DomainException("User already reviewed this media.");
            Reviews.Add(review);
        }
    }
}
