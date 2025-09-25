namespace ReolMarkedWPF.Models
{
    public class Rent
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int RentAgreementID { get; set; }
        public int ShelfVendorID { get; set; }
    }
}
