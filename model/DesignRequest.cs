namespace CustomClothing.Models;

public class DesignRequest
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string CustomerName { get; set; }
    public required string CustomerPhone { get; set; }
    public string? DeliveryAddress { get; set; } 
    public int ClothingCategoryId { get; set; }
    public ClothingCategory? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
}