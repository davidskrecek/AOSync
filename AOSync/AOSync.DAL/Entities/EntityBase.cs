namespace AOSync.DAL.Entities;

public record EntityBase
{
    public bool IsChanged { get; set; } = false;
    public bool IsCreated { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public bool Archived { get; set; } = false;
}