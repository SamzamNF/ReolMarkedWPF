using ReolMarkedWPF.Models;

namespace ReolMarkedWPF.Repositories
{
    public interface IPaymentMethodRepository
    {
        List<PaymentMethod> GetAllPaymentMethods();
        void AddPaymentMethod(PaymentMethod paymentMethod);
        void DeletePaymentMethod(PaymentMethod paymentMethod);
        void UpdatePaymentMethod(PaymentMethod paymentMethod);
    }
}