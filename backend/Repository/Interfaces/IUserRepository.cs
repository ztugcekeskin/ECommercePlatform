using WebAPI.Models;
namespace WebAPI.Repositories.Interfaces;

public interface IUserRepository
{
   Task<User?> GetByUsernameAsync(string username);
   Task<User?> GetByEmailAsync(string email);
   Task<User?> GetByIdAsync(int id);
   Task AddAsync(User user);
   Task SaveChangesAsync();
   Task Update(User user);
}