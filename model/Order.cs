namespace CustomClothing.model;

public class Order
{
    public int Id { get; set; }
    
    public int CompletedWorkId { get; set; }
    public CompletedWork? CompletedWork { get; set; }
    
    public required string CustomerName { get; set; }
    public required string CustomerPhone { get; set; }
    public string? DeliveryAddress { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.New;
}