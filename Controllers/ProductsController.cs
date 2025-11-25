using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("with-hierarchy")]
    public async Task<ActionResult<IEnumerable<ProductWithCategoryDto>>> GetAllWithHierarchy()
    {
        var products = await _productRepository.GetAllWithCategoryHierarchyAsync();
        return Ok(products);
    }

    [HttpGet("names-only")]
    public async Task<ActionResult<IEnumerable<ProductWithCategoryDto>>> GetAllNamesOnly()
    {
        var products = await _productRepository.GetAllWithCategoryNamesAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("{id}/with-hierarchy")]
    public async Task<ActionResult<ProductWithCategoryDto>> GetByIdWithHierarchy(int id)
    {
        var product = await _productRepository.GetByIdWithCategoryHierarchyAsync(id);
        
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        var createdProduct = await _productRepository.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> Update(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest();

        var exists = await _productRepository.ExistsAsync(id);
        if (!exists)
            return NotFound();

        var updatedProduct = await _productRepository.UpdateAsync(product);
        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _productRepository.DeleteAsync(id);
        
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
