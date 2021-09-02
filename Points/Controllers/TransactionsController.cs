using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Points.Models;
using Points.DTOs;

namespace Points.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TransactionsController : Controller
{
    private IAccountRepository accountRepository;

    public TransactionsController(IAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    [HttpPost]
    public ActionResult Post(TransactionDTO transactionDTO)
    {
        Account account = this.accountRepository.Find();
        Transaction transaction = transactionDTO.ToTransaction();

        if (!transaction.IsValidToPost(account, out string errorMessage))
        {
            return BadRequest(errorMessage);
        }

        account.CreateTransaction(transactionDTO.ToTransaction());
        this.accountRepository.Update(account);
        return Ok();
    }
}
