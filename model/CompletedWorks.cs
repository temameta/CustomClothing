namespace CustomClothing.model;

public class CompletedWork
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public int ClothingCategoryId { get; set; }
    public ClothingCategory? Category { get; set; }
}