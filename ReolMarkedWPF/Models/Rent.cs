using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReolMarkedWPF.Models
{
    public class Rent
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int RentID { get; set; }
        public int ShelfVendorID { get; set; }
    }
}
