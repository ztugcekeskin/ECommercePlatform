using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ReviewRepository _reviewRepository;

    public ReviewController(ReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
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
}