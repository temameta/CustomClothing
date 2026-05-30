namespace CustomClothing.Dto;

public record OrderResponseDto(
    int Id,
    string WorkName,
    string Status,
    DateTime OrderDate
);