using Domain.Base;
using Domain.Enums;

namespace Domain.Entity
{
    public record struct UserMediaId(Guid UserId, Guid MediaId);
    public class UserInteractions:Entity<UserMediaId>
    {
        public Guid UserId => Id.UserId;
        public Guid MediaId => Id.MediaId;
        public ERatingVote? RatingVote {  get; private set; }
        public ETypeInteractions? TypeInteractions { get; private set; }
        public DateTime InteractionDate { get; private set; }

        private UserInteractions(Guid userId, ETypeInteractions? typeInteractions, ERatingVote? ratingVote, Guid mediaId) : base(new UserMediaId(userId, mediaId))
        {
            TypeInteractions = typeInteractions;
            RatingVote = ratingVote;
            InteractionDate = DateTime.UtcNow;
        }
        internal void UpdateInteraction(ETypeInteractions? typeInteractions, ERatingVote? ratingVote)
        {
            TypeInteractions = typeInteractions;
            RatingVote = ratingVote;
            InteractionDate = DateTime.UtcNow;
        }
        internal void UpdateTypeInteractions(ETypeInteractions? typeInteractions)
        {
            TypeInteractions = typeInteractions;
            InteractionDate = DateTime.UtcNow;
        }
        internal void UpdateRatingVote(ERatingVote? ratingVote)
        {
            RatingVote = ratingVote;
            InteractionDate = DateTime.UtcNow;
        }
        public static UserInteractions Create(Guid userId, Guid mediaId, ETypeInteractions? typeInteractions, ERatingVote? ratingVote)
        {
            return new UserInteractions(userId, typeInteractions, ratingVote, mediaId);
        }
    }
}
