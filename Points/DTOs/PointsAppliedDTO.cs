
using Points.Models;

namespace Points.DTOs;
public class PointsAppliedDTO
{
    public PointsAppliedDTO()
    {
        this.Payer = string.Empty;
        this.Points = 0;
    }

    public string Payer { get; set; }

    public int Points { get; set; }
}

public static class PointsAppliedMapExtensions
{
    public static List<PointsAppliedDTO> ToPointsAppliedDTO(this IEnumerable<Transaction> transactions)
    {
        return transactions.Select(tx => new PointsAppliedDTO
        {
            Payer = tx.Payer,
            Points = tx.Points
        }).ToList();
    }
}
