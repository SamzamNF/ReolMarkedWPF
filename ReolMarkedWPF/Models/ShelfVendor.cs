namespace ReolMarkedWPF.Models
{
    public class ShelfVendor
    {
        public int ShelfVendorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        // Property til at holde det fulde navn med ID
        public string FullNameWithID
        {
            get { return $"{ShelfVendorID} {FirstName} {LastName}"; }
        }

        // Liste til at holde sælgerens tilknyttede betalingsmetoder
        public List<PaymentMethod> PaymentMethods { get; set; }

        public ShelfVendor()
        {
            PaymentMethods = new List<PaymentMethod>();
        }
    }
}