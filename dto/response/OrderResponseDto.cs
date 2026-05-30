namespace CustomClothing.dto.response;

public record OrderResponseDto(
    int Id,
    string WorkName,
    string Status,
    DateTime OrderDate
);