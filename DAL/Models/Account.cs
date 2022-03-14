using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ResourceId { get; set; }
        public string Product { get; set; }
        public string Iban { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }

        public List<Transactions> Transactions { get; set; }
    }
}
