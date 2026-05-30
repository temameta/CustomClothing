namespace CustomClothing.Dto;

public record FinalizeRequestDto(
    bool IsPublic, 
    string? OverrideName
);