using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Points.Models;
using Points.DTOs;

namespace Points.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SpendsController : Controller
{
    private IAccountRepository accountRepository;

    public SpendsController(IAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    [HttpPost]
    public ActionResult<List<PointsAppliedDTO>> Post(SpendDTO spend)
    {
        Account account = this.accountRepository.Find();

        if (spend.Points > account.CurrentBalance)
        {
            return BadRequest("Cannot spend more points than the account currently has.");
        }

        if (spend.Points <= 0)
        {
            return BadRequest("Points must be a positive value.");
        }

        List<Transaction> resultingTransactions = account.SpendPoints(spend.Points);
        this.accountRepository.Update(account);
        List<PointsAppliedDTO> pointsApplied = resultingTransactions.ToPointsAppliedDTO();
        return Ok(pointsApplied);
    }
}