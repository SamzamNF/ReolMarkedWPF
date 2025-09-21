using ReolMarkedWPF.Models;
using System.Collections.Generic;

namespace ReolMarkedWPF.Repositories
{
    public class SqlPaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly string _connectionString;

        public SqlPaymentMethodRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<PaymentMethod> GetAllPaymentMethods()
        {
            // Implementeres senere
            return new List<PaymentMethod>();
        }

        public void AddPaymentMethod(PaymentMethod paymentMethod)
        {
            // Implementeres senere
        }

        public void DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            // Implementeres senere
        }

        public void UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            // Implementeres senere
        }
    }
}