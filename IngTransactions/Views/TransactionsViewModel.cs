using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngTransactions.Views
{
    public class TransactionsViewModel
    {
        public string TotalAmount { get; set; }
        public string CategoryName { get; set; }
        public string Currency { get; set; }
    }
}
