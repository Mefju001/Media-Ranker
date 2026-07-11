namespace Domain.Base
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected init; }
        protected Entity() { }
        protected Entity(TId id) => Id = id;
    }
}
