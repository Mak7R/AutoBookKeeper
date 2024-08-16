namespace AutoBookKeeper.Web.Models.Transaction;

public class TransactionViewModel
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string NameIdentifier { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string? Description { get; set; }
    public DateTime TransactionTime { get; set; }
}