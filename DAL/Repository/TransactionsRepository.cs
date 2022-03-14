using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly SqliteDbContext context;
        public TransactionsRepository(SqliteDbContext _context)
        {
            context = _context;
        }
        public List<TransactionsReport> GetTransactionsByAccountId(int accountId)
        {
            return context.Transactions
                .Include(x => x.Account)
                .Where(x => x.Account.Id == accountId)
                .AsEnumerable()
                .GroupBy(x => x.CategoryId)
                .Select(rez => new TransactionsReport()
                {
                    CategoryName = rez.First().CategoryId,
                    TotalAmount = rez.Sum(x => x.Amount),
                    Currency = rez.Select(x => x.Account.Currency).First()
                })
                .ToList();
        }
    }
}
