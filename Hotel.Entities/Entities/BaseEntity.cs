using Hotel.Entities.DomainEvent.Intrefaces;

namespace Hotel.Entities.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; private set; }
        public bool IsDeleted { get; set; } = false;

        private List<IDomainEvent> _domainEvents;
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void MarkUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        protected void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents() => _domainEvents?.Clear();
    }
}
