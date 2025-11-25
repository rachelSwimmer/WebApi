using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WebApi.DTOs;

namespace WebApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<ProductWithCategoryDto>> GetAllWithCategoryHierarchyAsync()
    {
        // Load ALL categories with their relationships in one query (more efficient)
        var allCategories = await _context.Categories.ToListAsync();
        
        // Build in-memory lookup for children
        var categoryLookup = allCategories.ToDictionary(c => c.Id);
        var childrenLookup = allCategories
            .Where(c => c.ParentId.HasValue)
            .GroupBy(c => c.ParentId.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Load products with their categories
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        var result = new List<ProductWithCategoryDto>();

        foreach (var product in products)
        {
            if (product.Category != null)
            {
                // Build the hierarchy for this category
                BuildCategoryHierarchy(product.Category, childrenLookup, categoryLookup);
                
                result.Add(new ProductWithCategoryDto
                {
                    Id = product.Id,
                    ProductName = product.Name,
                    Category = MapCategoryWithChildrenToDto(product.Category)
                });
            }
        }

        return result;
    }

    public async Task<ProductWithCategoryDto?> GetByIdWithCategoryHierarchyAsync(int id)
    {
        // Load ALL categories
        var allCategories = await _context.Categories.ToListAsync();
        
        // Build lookups
        var categoryLookup = allCategories.ToDictionary(c => c.Id);
        var childrenLookup = allCategories
            .Where(c => c.ParentId.HasValue)
            .GroupBy(c => c.ParentId.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product?.Category == null)
            return null;

        BuildCategoryHierarchy(product.Category, childrenLookup, categoryLookup);

        return new ProductWithCategoryDto
        {
            Id = product.Id,
            ProductName = product.Name,
            Category = MapCategoryWithChildrenToDto(product.Category)
        };
    }

    private void BuildCategoryHierarchy(Category category, Dictionary<int, List<Category>> childrenLookup, Dictionary<int, Category> categoryLookup)
    {
        // Get children for this category from lookup
        if (childrenLookup.TryGetValue(category.Id, out var children))
        {
            category.Children = children;
            
            // Recursively build hierarchy for each child
            foreach (var child in children)
            {
                child.Parent = null; // Clear parent to avoid cycles
                BuildCategoryHierarchy(child, childrenLookup, categoryLookup);
            }
        }
        else
        {
            category.Children = new List<Category>();
        }
    }

    private async Task LoadAllChildrenRecursively(Category category)
    {
        // Check if children collection is already loaded, if not load it
        var childrenEntry = _context.Entry(category).Collection(c => c.Children);
        if (!childrenEntry.IsLoaded)
        {
            await childrenEntry.LoadAsync();
        }

        // Convert to list to avoid "collection modified" error during iteration
        var children = category.Children.ToList();
        
        // For each child, recursively load their children (and so on...)
        foreach (var child in children)
        {
            child.Parent = null; // Clear parent reference to avoid cycles
            await LoadAllChildrenRecursively(child); // Recursion!
        }
    }

    private CategoryWithChildrenDto MapCategoryWithChildrenToDto(Category category)
    {
        return new CategoryWithChildrenDto
        {
            Id = category.Id,
            Name = category.Name,
            Children = category.Children?.Select(c => MapCategoryWithChildrenToDto(c)).ToList() ?? new List<CategoryWithChildrenDto>()
        };
    }

    public async Task<IEnumerable<ProductWithCategoryDto>> GetAllWithCategoryNamesAsync()
    {
        // Load products with their categories
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        // Map to DTOs with only names
        var result = new List<ProductWithCategoryDto>();
        
        foreach (var product in products)
        {
            if (product.Category != null)
            {
                await LoadAllChildrenRecursively(product.Category);
                
                result.Add(new ProductWithCategoryDto
                {
                    Id = product.Id,
                    ProductName = product.Name,
                    Category = new CategoryWithChildrenDto
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name,
                        Children = product.Category.Children?.Select(c => MapCategoryWithChildrenToDto(c)).ToList() ?? new List<CategoryWithChildrenDto>()
                    }
                });
            }
        }

        return result;
    }

    private CategoryDto MapCategoryToDto(Category category)
    {
        return new CategoryDto
        {
            Name = category.Name,
            Children = category.Children?.Select(c => MapCategoryToDto(c)).ToList() ?? new List<CategoryDto>()
        };
    }
}
