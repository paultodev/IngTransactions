using DAL.Models;
using System.Collections.Generic;

namespace DAL.Repository
{
    public interface ITransactionsRepository
    {
        public List<TransactionsReport> GetTransactionsByAccountId(int accountId);
    }
}
