using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReolMarkedWPF.Models
{
    internal class Product
    {
        public int ProductID { get; set; }          // PK
        public int ShelfNumber {  get; set; }       // FK
        public string ProductName {  get; set; }
        public decimal UnitPrice {  get; set; }
        public int Amount {  get; set; }
    }
}
