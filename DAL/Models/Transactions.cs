using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public Categorylist CategoryId { get; set; }
        public string Iban { get; set; }
        public DateTime TransactionDate { get; set; }   
        public int AccountForeignKey { get; set; }
        public Account Account { get; set; }

    }
}
