using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReolMarkedWPF.Models
{
    public class Shelf
    {
        public int ShelfNumber { get; set; }
        public string ShelfType { get; set; }
        public decimal Price { get; set; }
        public int RentID { get; set; }
    }
}
