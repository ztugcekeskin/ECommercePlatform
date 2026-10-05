using WebAPI.Models;

namespace WebAPI.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByOrderIdAsync(int orderId);

    Task<Payment?> GetByIdAsync(int id);

    Task<Payment> AddAsync(Payment payment);

    Task UpdateAsync(Payment payment);
}