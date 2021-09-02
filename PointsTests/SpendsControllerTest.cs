using Microsoft.AspNetCore.Mvc;
using Points;
using Points.Controllers;
using Points.DTOs;
using Xunit;

namespace PointsTests
{
    [Collection("Sequential")]
    public class SpendsControllerTest
    {
        AccountRepository accountRepository;
        SpendsController controller;
        TransactionsController transactionsController;

        public SpendsControllerTest()
        {
            accountRepository = new AccountRepository();
            controller = new SpendsController(accountRepository);
            transactionsController = new TransactionsController(accountRepository);
        }

        // This could use more test coverage

        [Fact]
        public void SpendsPost_ReturnsOK()
        {
            accountRepository.Reset();

            // setup some points to spend
            TransactionDTO transactionDTO = new TransactionDTO
            {
                Payer = "DANNON",
                Points = 200,
                Timestamp = DateTime.Parse("2020-10-31T11:00:00Z")
            };

            transactionsController.Post(transactionDTO);

            var postResult = controller.Post(new Points.DTOs.SpendDTO { Points = 100 });

            Assert.IsType<OkObjectResult>(postResult.Result);
        }

        [Fact]
        public void SpendsPost_ReturnsBadRequest_ForSpendingPointsOverBalance()
        {
            accountRepository.Reset();

            var postResult = controller.Post(new Points.DTOs.SpendDTO { Points = 100 });

            Assert.IsType<BadRequestObjectResult>(postResult.Result);
        }

        [Fact]
        public void SpendsPost_ReturnsBadRequest_ForSpendingNegativePoints()
        {
            accountRepository.Reset();

            var postResult = controller.Post(new Points.DTOs.SpendDTO { Points = -100 });

            Assert.IsType<BadRequestObjectResult>(postResult.Result);
        }
    }
}
