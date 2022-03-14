using DAL.Repository;
using IngTransactions.Services;
using IngTransactions.Views;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IngTransactions.Controllers
{

    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private ITransactionsRepository transactionsRepository;
        private IAccountRepository accountRepository;
        private IClientService clientService;
        public HomeController(ITransactionsRepository _transactionsRepository, IAccountRepository _accountRepository, IClientService _clientService)
        {
            transactionsRepository = _transactionsRepository;
            accountRepository = _accountRepository;
            clientService = _clientService;
        }

        [HttpGet]
        [Route("api/transactions/report")]
        public List<TransactionsViewModel> GetTransactionsAsync()
        {
            var accountId = accountRepository.GetAccounts().First().Id;
            var resultFromDb = transactionsRepository.GetTransactionsByAccountId(accountId);
            var result = new List<TransactionsViewModel>();
            foreach (var transaction in resultFromDb)
            {
                result.Add(new TransactionsViewModel()
                {
                    TotalAmount = Math.Round(transaction.TotalAmount, 3).ToString("0.##"),
                    CategoryName = transaction.CategoryName.ToString(),
                    Currency = transaction.Currency
                });
            }
            return result;
        }

        [HttpGet]
        [Route("api/accounts")]
        public AccountsViewModel GetAccounts()
        {
            var accounts = accountRepository.GetAccounts();
            var result = new AccountsViewModel();
            foreach (var acc in accounts)
            {
                result.Accounts.Add(new AccountViewModel()
                {
                    ResourceId = acc.ResourceId,
                    Product = acc.Product,
                    Iban = acc.Iban,
                    Name = acc.Name,
                    Currency = acc.Currency
                });
            }
            return result;
        }

        [HttpGet]
        [Route("api/account")]
        public AccountViewModel GetAcountWithToken()
        {
            var result = new AccountViewModel();
            clientService.Authenticate();
            return result;
        }
    }
}
