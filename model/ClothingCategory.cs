namespace CustomClothing.Models;

public class ClothingCategory
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<CompletedWork> CompletedWorks { get; set; } = [];
}