using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using System.IO;
using Microsoft.AspNetCore.Http;

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
    public async Task<IActionResult> GetAllProducts(string? search)
    {
    if (!string.IsNullOrWhiteSpace(search))
    {
        var searchedProducts =
            await _productRepository.SearchAsync(search);

        return Ok(searchedProducts);
    }
    var products = await _productRepository.GetAllAsync();

    return Ok(products);
    }
    
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetProductsByCategory(string category)
    {
    var products = await _productRepository.GetByCategoryAsync(category);

    return Ok(products);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterProducts(
    string? search,
    string? category,
    string? sort)
    {
    var products = await _productRepository.FilterAsync(
        search,
        category,
        sort); 
    return Ok(products);
    }

    [HttpGet("{id}")]
public async Task<IActionResult> GetProduct(int id)
{
    var product = await _productRepository.GetProductWithSellerAsync(id);

    if (product == null)
    {
        return NotFound(new
        {
            message = "Ürün bulunamadı."
        });
    }

    return Ok(new
    {
        id = product.Id,
        name = product.Name,
        description = product.Description,
        price = product.Price,
        stock = product.Stock,
        imageUrl = product.ImageUrl,
        category = product.Category,
        seller = product.Seller == null
            ? null
            : new
            {
                id = product.Seller.Id,
                username = product.Seller.Username
            }
    });
}

    [HttpGet("seller/{sellerId}")]
    public async Task<IActionResult> GetSellerProducts(
    int sellerId,
    string? search)
    {
    if (!string.IsNullOrWhiteSpace(search))
    {
        var products =
            await _productRepository.SearchBySellerAsync(
                sellerId,
                search);

        return Ok(products);
    }
    var sellerProducts =
        await _productRepository.GetBySellerIdAsync(sellerId);

    return Ok(sellerProducts);
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
            ImageUrl = dto.ImageUrl,
            Category = dto.Category
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return Ok(new
        {
            message = "Ürün başarıyla eklendi.",
            productId = product.Id
        } );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
    var product = await _productRepository.GetByIdAsync(id);

    if (product == null)
        return NotFound(new 
        { message = "Ürün bulunamadı." });

    await _productRepository.Delete(product);
    await _productRepository.SaveChangesAsync();

    return Ok(new { message = "Ürün silindi." });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return NotFound(new 
            { message = "Ürün bulunamadı." });

        product.SellerId = dto.SellerId;
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.ImageUrl = dto.ImageUrl;
        product.Category = dto.Category;
        await _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        return Ok(new 
        { message = "Ürün güncellendi." });
    }

    [HttpPost("{id}/upload-photo")]
    public async Task<IActionResult> UploadPhoto(
    int id,
    [FromForm] UploadProductPhotoDto dto)
    {
    var product = await _productRepository.GetByIdAsync(id);

    if (product == null)
    {
        return NotFound(new
        {
            message = "Ürün bulunamadı."
        });
    }

    if (dto.Image == null || dto.Image.Length == 0)
    {
        return BadRequest(new
        {
            message = "Lütfen bir fotoğraf seçin."
        });
    }

    var allowedExtensions = new[] { ".jpg", ".jpeg" };
    var extension = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();

    if (!allowedExtensions.Contains(extension))
    {
    return BadRequest(new
    {
        message = "Sadece JPG ve JPEG formatındaki fotoğraflar kabul edilir."
    });
    }

    var uploadsFolder = Path.Combine(
        Directory.GetCurrentDirectory(),
        "Uploads",
        "Products");

    if (!Directory.Exists(uploadsFolder))
    {
        Directory.CreateDirectory(uploadsFolder);
    }

    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);

    var filePath =Path.Combine(uploadsFolder, fileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await dto.Image.CopyToAsync(stream);
    }

    product.ImageUrl = "/uploads/Products/" + fileName;

    await _productRepository.Update(product);
    await _productRepository.SaveChangesAsync();

    return Ok(new
    {
        message = "Ürün fotoğrafı yüklendi.",
        imageUrl = product.ImageUrl
    });
}

}