namespace Shared;

public abstract class Entity<TEntity>
{
    public TEntity Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    protected Entity(TEntity id)
    {
        Id = id;
    }

    protected Entity()
    {
    }
}
