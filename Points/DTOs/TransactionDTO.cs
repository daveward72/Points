
using Points.Models;

namespace Points.DTOs;
public class TransactionDTO
{
    public TransactionDTO()
    {
        this.Payer = string.Empty;
        this.Points = 0;
        this.Timestamp = DateTime.MinValue;
    }

    public string Payer { get; set; }

    public int Points { get; set; }

    public DateTime Timestamp { get; set; }
}
public static class TransactionMapExtensions
{
    public static Transaction ToTransaction(this TransactionDTO transactionDTO)
    {
        return new Transaction
        {
            Payer = transactionDTO.Payer,
            Points = transactionDTO.Points,
            Timestamp = transactionDTO.Timestamp
        };
    }
}
