namespace ReolMarkedWPF.Models
{
    // Denne klasse bruges til at kombinere data fra TransactionProduct og Product
    public class TransactionDetailItem
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }
    }
}