using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReolMarkedWPF.Models.AccountingModels;

namespace ReolMarkedWPF.Repositories.AccountingRepository
{
    public class SQLAccountingRepository : IAccountingRepository
    {
        public async Task<List<AccountingResult>> GetAccountingData(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
