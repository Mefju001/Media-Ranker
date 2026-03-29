using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base
{
    public abstract class AggregateRoot<TId>:Entity<TId>
    {
        private readonly List<object> _domainEvents = new();
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(object domainEvent) => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected AggregateRoot() : base() { }

        protected AggregateRoot(TId id) : base(id) { }
    }
}
