namespace WebApi.DTOs;

public class ProductWithCategoryDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public CategoryWithChildrenDto Category { get; set; }
}

public class CategoryWithChildrenDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CategoryWithChildrenDto> Children { get; set; } = new List<CategoryWithChildrenDto>();
}

public class CategoryDto
{
    public string Name { get; set; }
    public List<CategoryDto> Children { get; set; } = new List<CategoryDto>();
}
