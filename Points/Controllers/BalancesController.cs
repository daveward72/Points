using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Points.Models;

namespace Points.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BalancesController : ControllerBase
{
    private IAccountRepository accountRepository;

    public BalancesController(IAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    [HttpGet]
    public ActionResult<Dictionary<string, int>> Get()
    {
        Account account = this.accountRepository.Find();
        return Ok(account.GetPayerBalances());
    }
}
