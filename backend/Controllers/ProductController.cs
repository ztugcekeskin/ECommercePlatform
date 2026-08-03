using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productRepository.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("seller/{sellerId}")]
    public async Task<IActionResult> GetSellerProducts(int sellerId)
    {
        var products = await _productRepository.GetBySellerIdAsync(sellerId);

        return Ok(products);
    }
    

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto dto)
    {
        var product = new Product
        {
            SellerId = dto.SellerId,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return Ok(new
        {
            message = "Ürün başarıyla eklendi."
        } );
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
    var product = await _productRepository.GetByIdAsync(id);

    if (product == null)
        return NotFound(new { message = "Ürün bulunamadı." });

    await _productRepository.Delete(product);
    await _productRepository.SaveChangesAsync();

    return Ok(new { message = "Ürün silindi." });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return NotFound(new { message = "Ürün bulunamadı." });

        product.SellerId = dto.SellerId;
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.ImageUrl = dto.ImageUrl;

        await _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        return Ok(new { message = "Ürün güncellendi." });
    }

}