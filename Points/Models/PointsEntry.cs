
namespace Points.Models;
public class PointsEntry
{
    public string Payer { get; set; }

    public DateTime AcquiredDate { get; set; }

    public int PointBalance { get; set;  }

    public Transaction ApplyPoints(int pointsToApply)
    {
        this.PointBalance -= pointsToApply;

        DateTime timestamp = DateTime.UtcNow;

        return new Transaction
        {
            Payer = this.Payer,
            Points = -1 * pointsToApply,
            Timestamp = timestamp
        };
    }
}

public static class ListExtensions
{
    public static List<Transaction> ApplyPoints(this IEnumerable<PointsEntry> unspentPointsEntries, int points)
    {
        int pointsToSpend = points;

        var orderedEntries = unspentPointsEntries.OrderBy(pe => pe.AcquiredDate);

        var transactions = new List<Transaction>();

        foreach (var entry in orderedEntries)
        {
            int pointsApplied = Math.Min(pointsToSpend, entry.PointBalance);
            var transaction = entry.ApplyPoints(pointsApplied);
            
            transactions.Add(transaction);

            pointsToSpend -= pointsApplied;

            if (pointsToSpend == 0)
            {
                break;
            }
        }

        return transactions;
    }

    public static IEnumerable<PointsEntry> GetUnspent(this IEnumerable<PointsEntry> pointsEntries)
    {
        return pointsEntries.Where(pe => pe.PointBalance > 0);
    }

    public static IEnumerable<PointsEntry> GetUnspent(this IEnumerable<PointsEntry> pointsEntries, string payer)
    {
        return pointsEntries.Where(pe => pe.PointBalance > 0 && pe.Payer.ToLower() == payer.ToLower());
    }
}