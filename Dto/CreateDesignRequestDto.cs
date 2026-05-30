namespace CustomClothing.Dto;

public record CreateDesignRequestDto(
    string Title, 
    string Description, 
    string CustomerName, 
    string CustomerPhone, 
    string? DeliveryAddress,
    int CategoryId
);