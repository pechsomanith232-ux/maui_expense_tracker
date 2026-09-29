using SQLite;

namespace expense_tracker.Models;

[Table("expenses")]
public class Expense
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    [MaxLength(50)]
    public string Category { get; set; } = "General";

    public DateTime Date { get; set; } = DateTime.UtcNow;
}