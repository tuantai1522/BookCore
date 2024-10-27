namespace Shared;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity()
    {
    }
}
