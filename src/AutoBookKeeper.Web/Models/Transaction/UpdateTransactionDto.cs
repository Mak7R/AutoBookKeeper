namespace AutoBookKeeper.Web.Models.Transaction;

public class UpdateTransactionDto
{
    public string NameIdentifier { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string? Description { get; set; }
    public DateTime? TransactionTime { get; set; }
}