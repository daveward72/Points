using Microsoft.AspNetCore.Mvc;
using Points;
using Points.Controllers;
using Points.DTOs;
using Xunit;

namespace PointsTests
{
    [Collection("Sequential")]
    public class TransactionsControllerTest
    {
        AccountRepository accountRepository;
        TransactionsController controller;

        public TransactionsControllerTest()
        {
            accountRepository = new AccountRepository();
            controller = new TransactionsController(accountRepository);
        }

        // This could use more test coverage

        [Fact]
        public void TransactionsPost_ReturnsOK()
        {
            accountRepository.Reset();

            TransactionDTO transactionDTO = new TransactionDTO
            {
                Payer = "DANNON",
                Points = 200,
                Timestamp = DateTime.Parse("2020-10-31T11:00:00Z")
            };

            var  postResult = controller.Post(transactionDTO);
            Assert.IsType<OkResult>(postResult);
        }

        [Fact]
        public void TransactionsPost_ReturnsBadRequest()
        {
            accountRepository.Reset();

            TransactionDTO transactionDTO = new TransactionDTO
            {
                Payer = "DANNON",
                Points = -200,
                Timestamp = DateTime.Parse("2020-10-31T11:00:00Z")
            };

            var postResult = controller.Post(transactionDTO);
            Assert.IsType<BadRequestObjectResult>(postResult);
        }
    }
}
