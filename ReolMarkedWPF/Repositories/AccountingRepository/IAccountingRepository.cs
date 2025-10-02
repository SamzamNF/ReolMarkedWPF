using ReolMarkedWPF.Models.AccountingModels;

namespace ReolMarkedWPF.Repositories.AccountingRepository
{
    public interface IAccountingRepository
    {
        public Task<List<AccountingResult>> GetAccountingData(DateOnly startDate, DateOnly endDate);
        public Task<List<AccountingResult>> GetAllSales(int? ShelfVendorID);
    }
}
