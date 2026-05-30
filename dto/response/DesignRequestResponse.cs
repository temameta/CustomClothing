namespace CustomClothing.dto.response;

public record DesignRequestResponse(
    int Id,
    string Title,
    string Description,
    string CustomerName,
    string CustomerPhone,
    string? DeliveryAddress,
    string CategoryName,
    string Status
);