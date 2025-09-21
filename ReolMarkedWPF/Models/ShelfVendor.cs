namespace ReolMarkedWPF.Models
{
    public class ShelfVendor
    {
        public int ShelfVendorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        // Liste til at holde sælgerens tilknyttede betalingsmetoder
        public List<PaymentMethod> PaymentMethods { get; set; }

        public ShelfVendor()
        {
            PaymentMethods = new List<PaymentMethod>();
        }
    }
}