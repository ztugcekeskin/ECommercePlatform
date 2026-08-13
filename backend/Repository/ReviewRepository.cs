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

    public async Task<bool> HasUserReviewedProductAsync(
    int userId,
    int productId)
    {
    return await _reviews
        .Find(r =>
            r.UserId == userId &&
            r.ProductId == productId)
        .AnyAsync();
    }

    public async Task AddAsync(Review review)
    {
        await _reviews.InsertOneAsync(review);
    }

    public async Task<List<Review>> GetByProductIdsAsync(List<int> productIds)
    {
    return await _reviews
        .Find(r => productIds.Contains(r.ProductId))
        .SortByDescending(r => r.CreatedAt)
        .ToListAsync();
    }   
}