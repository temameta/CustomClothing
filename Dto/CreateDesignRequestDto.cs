namespace CustomClothing.Dto;

/// <summary> Данные для создания заявки на дизайн </summary>
public record CreateDesignRequestDto
{
    /// <summary> Краткое название задумки </summary>
    /// <example>Куртка с черепом</example>
    public required string Title { get; init; }

    /// <summary> Подробное описание дизайна </summary>
    /// <example>Нужна кожаная куртка с вышивкой на спине</example>
    public required string Description { get; init; }

    /// <summary> Имя клиента </summary>
    /// <example>Иван Иванов</example>
    public required string CustomerName { get; init; }

    /// <summary> Телефон для связи с менеджером </summary>
    /// <example>+79001112233</example>
    public required string CustomerPhone { get; init; }

    /// <summary> Адрес доставки готового изделия </summary>
    /// <example>г. Москва, ул. Арбат, д. 1</example>
    public string? DeliveryAddress { get; init; }

    /// <summary> ID категории одежды </summary>
    /// <example>1</example>
    public required int CategoryId { get; init; }
}