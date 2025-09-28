using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public interface ITransactionProductRepository
    {
        List<TransactionProduct> GetAllTransactionProducts();
        void AddTransactionProduct(TransactionProduct transactionProduct);
        void DeleteTransactionProduct(TransactionProduct transactionProduct);
        void UpdateTransactionProduct(TransactionProduct transactionProduct);
    }
}
