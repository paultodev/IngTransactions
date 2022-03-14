using DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SqliteDbContext context;
        public AccountRepository(SqliteDbContext _context)
        {
            context = _context;
        }
        public List<Account> GetAccounts()
        {
            return context.Account.ToList();
        }
    }
}
