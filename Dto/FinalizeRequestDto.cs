namespace CustomClothing.Dto;

/// <summary> Параметры финализации заявки </summary>
public record FinalizeRequestDto
{
    /// <summary> Нужно ли публиковать работу в общий каталог </summary>
    /// <example>true</example>
    public required bool IsPublic { get; init; }

    /// <summary> Новое название для каталога (если оставить пустым, возьмется из заявки) </summary>
    /// <example>Кожанка с драконом (VIP)</example>
    public string? OverrideName { get; init; }
}