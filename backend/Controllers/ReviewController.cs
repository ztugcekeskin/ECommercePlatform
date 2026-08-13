using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ReviewRepository _reviewRepository;
    private readonly IProductRepository _productRepository;
    public ReviewController(
        ReviewRepository reviewRepository, 
        IProductRepository productRepository)
    {
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProductReviews(int productId)
    {
        var reviews = await _reviewRepository.GetByProductIdAsync(productId);

        return Ok(reviews);
    }

    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] Review review)
    {

        var alreadyReviewed =
        await _reviewRepository.HasUserReviewedProductAsync(
        review.UserId,
        review.ProductId
    );

    if (alreadyReviewed)
    {
    return BadRequest(new
    {
        message = "Bu ürünü daha önce değerlendirdiniz."
    });
    }

        if (review.Rating < 1 || review.Rating > 5)
        {
            return BadRequest(new
            {
                message = "Puan 1 ile 5 arasında olmalıdır."
            });
        }

        if (string.IsNullOrWhiteSpace(review.Comment))
        {
            return BadRequest(new
            {
                message = "Yorum boş bırakılamaz."
            });
        }

        review.Id = "";
        review.CreatedAt = DateTime.UtcNow;

        await _reviewRepository.AddAsync(review);

        return Ok(new
        {
            message = "Yorum başarıyla eklendi."
        });
    }

    [HttpGet("seller/{sellerId}")]
    public async Task<IActionResult> GetSellerReviews(int sellerId)
    {
    var products = await _productRepository.GetBySellerIdAsync(sellerId);

    var productIds = products
        .Select(p => p.Id)
        .ToList();

    if (!productIds.Any())
    {
        return Ok(new List<object>());
    }

    var reviews = await _reviewRepository
        .GetByProductIdsAsync(productIds);

    var result = reviews.Select(review =>
    {
        var product = products
            .FirstOrDefault(p => p.Id == review.ProductId);

        return new
        {
            id = review.Id,
            userId = review.UserId,
            userName = review.UserName,
            rating = review.Rating,
            comment = review.Comment,
            createdAt = review.CreatedAt,

            product = product == null ? null : new
            {
                id = product.Id,
                name = product.Name,
                price = product.Price,
                imageUrl = product.ImageUrl
            }
        };
    });
    return Ok(result);
    }

}