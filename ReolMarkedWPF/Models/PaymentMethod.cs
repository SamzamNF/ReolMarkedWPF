namespace ReolMarkedWPF.Models
{
    public class PaymentMethod
    {
        public AccountPaymentOption PaymentOption { get; set; }
        public string PaymentInfo { get; set; }
        public int ShelfVendorID { get; set; }
    }
}