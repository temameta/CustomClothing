namespace CustomClothing.Dto;

public record CreateOrderDto(
    int CompletedWorkId,
    string CustomerName,
    string CustomerPhone,
    string? DeliveryAddress
);