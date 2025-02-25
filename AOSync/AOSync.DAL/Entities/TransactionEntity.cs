using System.ComponentModel.DataAnnotations;

namespace AOSync.DAL.DB;

public class TransactionEntity
{
    [Key] 
    public string Id { get; set; } = string.Empty;

    [Required]
    public DateTime DateAdded { get; set; }
}