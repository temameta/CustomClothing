namespace CustomClothing.dto.request;

/// <summary> Данные для создания новой категории </summary>
public record CreateCategoryDto
{
    /// <example>Худи</example>
    public string Name { get; init; }
}