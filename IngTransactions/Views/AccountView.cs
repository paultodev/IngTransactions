using System.Collections.Generic;

namespace IngTransactions.Views
{
    public class AccountsViewModel
    {
        public AccountsViewModel()
        {
            Accounts = new List<AccountViewModel>();
        }
        public List<AccountViewModel> Accounts { get; set; }
        
    }
}
