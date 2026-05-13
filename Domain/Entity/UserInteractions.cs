using Domain.Base;
using Domain.Enums;

namespace Domain.Entity
{
    public class UserInteractions:Entity<Guid>
    {
        public Guid UserId { get; init; }
        public Guid MediaId {  get; init; }
        public ERatingVote? RatingVote {  get; private set; }
        public ETypeInteractions? TypeInteractions { get; private set; }
        public DateTime InteractionDate { get; private set; }
        private UserInteractions() { }
        private UserInteractions(Guid userId, ETypeInteractions? typeInteractions, ERatingVote? ratingVote, Guid mediaId, Guid? id=null)
        {
            Id = id ?? Guid.NewGuid();
            MediaId = mediaId;
            UserId = userId;
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
