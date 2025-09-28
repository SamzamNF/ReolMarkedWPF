namespace ReolMarkedWPF.Models
{
    public class Shelf
    {
        public int ShelfNumber { get; set; }
        public int? RentAgreementID { get; set; }
        public string ShelfType { get; set; }
        public decimal Price { get; set; }
        public double X { get; set; } // X-koordinat på Canvas
        public double Y { get; set; } // Y-koordinat på Canvas
    }
}