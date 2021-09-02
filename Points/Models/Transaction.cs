
namespace Points.Models;
public class Transaction
{
    public string Payer { get; set; }

    public int Points { get; set; }

    public DateTime Timestamp { get; set; }

    public bool IsValidToPost(Account account, out string errorMessage)
    {
        errorMessage = "";

        if (this.Points < 0 && Math.Abs(this.Points) > account.GetPayerBalance(this.Payer))
        {
            errorMessage = "Cannot post transaction that would set payer points to a negative value.";
        }

        return string.IsNullOrEmpty(errorMessage);
    }
}
