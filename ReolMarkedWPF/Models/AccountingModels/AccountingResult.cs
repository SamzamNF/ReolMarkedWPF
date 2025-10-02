using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReolMarkedWPF.Models.AccountingModels
{
    public class AccountingResult
    {
        // ShelfVendorID nullable
        public int? ShelfVendorID { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string PaymentInfo { get; set; }
        public decimal TotalRent { get; set; }
        public decimal TotalSale { get; set; }
        public decimal AfterComission { get; set; }
        public decimal Payment { get; set; }
    }
}
