using MongoDB.Driver;
using WebAPI.Models;

namespace WebAPI.Repositories;

public class ReviewRepository
{
    private readonly IMongoCollection<Review> _reviews;

    public ReviewRepository(IMongoDatabase database)
    {
        _reviews = database.GetCollection<Review>("Reviews");
    }

    public async Task<List<Review>> GetByProductIdAsync(int productId)
    {
        return await _reviews
            .Find(r => r.ProductId == productId)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Review review)
    {
        await _reviews.InsertOneAsync(review);
    }
}