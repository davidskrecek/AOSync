using System.ComponentModel.DataAnnotations;

namespace AOSync.DAL.Entities;

public class TransactionEntity
{
    [Key] 
    public string Id { get; set; } = string.Empty;

    [Required]
    public DateTime DateAdded { get; set; }
}