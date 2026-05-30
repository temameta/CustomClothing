namespace CustomClothing.Dto;

public record CreateDesignRequestDto(
    string Title, 
    string Description, 
    string CustomerPhone, 
    int CategoryId
);