using System.ComponentModel.DataAnnotations;

namespace AOSync.DAL.Entities;

public record TransactionEntity
{
    [Key] 
    [Required]
    public string Id { get; set; }

    [Required]
    public DateTime DateAdded { get; set; }
}