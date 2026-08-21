using Domain.Enums;

namespace Domain.Extensions
{
    public static class ERatingVoteExtensions
    {
        public static ERatingVote toEnum(this string value)
            => value switch
            {
                "Liked" => ERatingVote.Liked,
                "Disliked" => ERatingVote.Disliked,
                _ => throw new ArgumentException($"Invalid value for ERatingVote: {value}")
            };
        /*{
            return switch
            {
                "Liked" => ERatingVote.Liked,
                "Disliked" => ERatingVote.Disliked,
                _ => throw new ArgumentException($"Invalid value for ERatingVote: {value}")
            }*/
    }
}
