using Microsoft.AspNetCore.Mvc;
using Points;
using Points.Controllers;
using Xunit;

namespace PointsTests;

[Collection("Sequential")]
public class BalancesControllerTest
{
    AccountRepository accountRepository;
    BalancesController controller;

    public BalancesControllerTest()
    {
        accountRepository = new AccountRepository();
        controller = new BalancesController(accountRepository);
    }

    [Fact]
    public void Get_ReturnsOK()
    {
        accountRepository.Reset();

        var getResult = controller.Get();

        Assert.IsType<OkObjectResult>(getResult.Result);
    }
}