using Microsoft.AspNetCore.Mvc;
using Points;
using Points.Controllers;
using Points.DTOs;
using Xunit;

namespace PointsTests
{
    [Collection("Sequential")]
    public class ExampleTest
    {
        AccountRepository accountRepository;
        BalancesController balancesController;
        SpendsController spendsController;
        TransactionsController transactionsController;

        public ExampleTest()
        {
            accountRepository = new AccountRepository();
            accountRepository.Reset();

            balancesController = new BalancesController(accountRepository);
            spendsController = new SpendsController(accountRepository);
            transactionsController = new TransactionsController(accountRepository);
        }

        [Fact]
        public void FetchExample_ExpectedResults()
        {
            accountRepository.Reset();

            PostTransaction("DANNON", 1000, DateTime.Parse("2020-11-02T14:00:00Z"));
            PostTransaction("UNILEVER", 200, DateTime.Parse("2020-10-31T11:00:00Z"));
            PostTransaction("DANNON", -200, DateTime.Parse("2020-10-31T15:00:00Z"));
            PostTransaction("MILLER COORS", 10000, DateTime.Parse("2020-11-01T14:00:00Z"));
            PostTransaction("DANNON", 300, DateTime.Parse("2020-10-31T10:00:00Z"));

            SpendDTO spendDTO = new SpendDTO { Points = 5000 };

            OkObjectResult spendOkResult = spendsController.Post(spendDTO).Result as OkObjectResult;

            List<PointsAppliedDTO> pointsAppliedDTOs = spendOkResult.Value as List<PointsAppliedDTO>;
            Assert.Equal(-100, pointsAppliedDTOs.FirstOrDefault(dto => dto.Payer == "DANNON").Points);
            Assert.Equal(-200, pointsAppliedDTOs.FirstOrDefault(dto => dto.Payer == "UNILEVER").Points);
            Assert.Equal(-4700, pointsAppliedDTOs.FirstOrDefault(dto => dto.Payer == "MILLER COORS").Points);

            OkObjectResult balanceOkResult = balancesController.Get().Result as OkObjectResult;
            Dictionary<string, int> payerBalances = balanceOkResult.Value as Dictionary<string, int>;
            Assert.Equal(1000, payerBalances["DANNON"]);
            Assert.Equal(0, payerBalances["UNILEVER"]);
            Assert.Equal(5300, payerBalances["MILLER COORS"]);
        }

        private void PostTransaction(string payer, int points, DateTime timestamp)
        {
            TransactionDTO transactionDTO = new TransactionDTO
            {
                Payer = payer,
                Points = points,
                Timestamp = timestamp
            };

            transactionsController.Post(transactionDTO);
        }
    }
}
