namespace ReolMarkedWPF.Models
{
    public class ShelfVendor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ShelfVendorID { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }

        public ShelfVendor()
        {
            PaymentMethods = new List<PaymentMethod>();
        }
    }
}