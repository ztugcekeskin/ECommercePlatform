using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }
}