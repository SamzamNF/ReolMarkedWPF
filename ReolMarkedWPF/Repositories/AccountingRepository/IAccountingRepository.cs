using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ReolMarkedWPF.Models.AccountingModels;

namespace ReolMarkedWPF.Repositories.AccountingRepository
{
    public interface IAccountingRepository
    {
        public Task<List<AccountingResult>> GetAccountingData(DateOnly startDate, DateOnly endDate);
        public Task<List<AccountingResult>> GetAllSales(int? ShelfVendorID);
    }
}
