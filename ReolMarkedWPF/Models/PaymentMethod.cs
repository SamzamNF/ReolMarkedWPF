namespace ReolMarkedWPF.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public int ShelfVendorID { get; set; }
        public AccountPaymentOption PaymentOption { get; set; }
        public string PaymentInfo { get; set; }
    }
}