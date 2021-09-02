
namespace Points.Models;
public class Account
{
    private readonly object accountDataUpdateLock = new object();

    private List<Transaction> transactionHistory = new List<Transaction>();
    private List<PointsEntry> pointsEntries = new List<PointsEntry>();

    public int CurrentBalance
    {
        get
        {
            return this.pointsEntries.Sum(pe => pe.PointBalance);
        }
    }

    public void CreateTransaction(Transaction transaction)
    {
        lock (accountDataUpdateLock)
        {
            this.transactionHistory.Add(transaction);

            var maxTimestamp = this.transactionHistory.Max(tx => tx.Timestamp);

            if (transaction.Timestamp <= maxTimestamp)
            {
                // Special case: back dated transactions require points entries to be recomputed
                var recomputedEntries = ReapplyPoints();

                this.pointsEntries = recomputedEntries;
            }
            else
            {
                ApplyPoints(transaction, this.pointsEntries);
            }
        }
    }
    public List<Transaction> SpendPoints(int points)
    {
        lock (accountDataUpdateLock)
        {
            IEnumerable<PointsEntry> unspentPointEntries = this.pointsEntries.GetUnspent();
            List<Transaction> spendPointsTransactions = unspentPointEntries.ApplyPoints(points);
            this.transactionHistory.AddRange(spendPointsTransactions);

            return spendPointsTransactions;
        }
    }

    public Dictionary<string, int> GetPayerBalances()
    {
        return this.pointsEntries.GroupBy(pe => pe.Payer)
                .ToDictionary(g => g.Key, g => g.Sum(pe => pe.PointBalance));
    }

    public int GetPayerBalance(string payer)
    {
        return this.pointsEntries.Where(pe => pe.Payer.ToLower() == payer.ToLower()).Sum(pe => pe.PointBalance);
    }

    // Since transaction timestamp can be backdated, may need to run through transaction history and redo the point reduction adjustments
    private List<PointsEntry> ReapplyPoints()
    {
        List<PointsEntry> adjustedPointEntries = new List<PointsEntry>();  

        foreach (var transaction in this.transactionHistory.OrderBy(tx => tx.Timestamp))
        {
            ApplyPoints(transaction, adjustedPointEntries);
        }

        return adjustedPointEntries;
    }

    private void ApplyPoints(Transaction transaction, List<PointsEntry> pointsEntries)
    {
        if (transaction.Points > 0)
        {
            pointsEntries.Add(new PointsEntry { Payer = transaction.Payer, PointBalance = transaction.Points, AcquiredDate = transaction.Timestamp });
        }
        else if (transaction.Points < 0)
        {
            IEnumerable<PointsEntry> payerUnspentPointsEntries = pointsEntries.GetUnspent(transaction.Payer);
            payerUnspentPointsEntries.ApplyPoints(-1 * transaction.Points);
        }
    }




}