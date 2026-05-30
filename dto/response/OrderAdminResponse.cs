namespace CustomClothing.dto.response;

public record OrderAdminResponse(
    int Id, 
    string WorkName, 
    string CustomerName, 
    string CustomerPhone, 
    string? DeliveryAddress, 
    DateTime OrderDate
);