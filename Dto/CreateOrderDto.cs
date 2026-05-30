namespace CustomClothing.Dto;

/// <summary> Данные для заказа готовой вещи из каталога </summary>
public record CreateOrderDto
{
    /// <summary> ID выполненной работы из каталога </summary>
    /// <example>2</example>
    public required int CompletedWorkId { get; init; }

    /// <summary> Имя покупателя </summary>
    /// <example>Мария Петрова</example>
    public required string CustomerName { get; init; }

    /// <summary> Телефон покупателя </summary>
    /// <example>88005553535</example>
    public required string CustomerPhone { get; init; }

    /// <summary> Адрес доставки </summary>
    /// <example>г. Казань, ул. Мира, д. 5</example>
    public string? DeliveryAddress { get; init; }
}