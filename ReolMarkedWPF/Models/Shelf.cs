namespace ReolMarkedWPF.Models
{
    public class Shelf
    {
        public int ShelfNumber { get; set; }        // PK
        public int? RentAgreementID { get; set; }    // FK NULLABLE INT
        public string ShelfType { get; set; }
        public decimal Price { get; set; }
    }
}
