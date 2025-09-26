using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReolMarkedWPF.Models
{

    //Model til TransactionProduct

    public class TransactionProduct
    {
        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount { get; set; }
    }
}
