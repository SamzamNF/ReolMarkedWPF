namespace ReolMarkedWPF.Repositories
{
    public interface ITransactionRepository<T> where T : class
    {
        List<T> GetAllTransactions();
        int AddTransaction(T transaction);
        void DeleteTransaction(T transaction);
        void UpdateTransaction(T transaction);

    }
}
