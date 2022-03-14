namespace IngTransactions.Views
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string ResourceId { get; set; }
        public string Product { get; set; }
        public string Iban { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
    }
}