using CustomClothing.Dto;

namespace CustomClothing.Service;

public interface IClothingService
{
    Task<IEnumerable<CompletedWorkDto>> GetPublicCatalogAsync();
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task CreateRequestAsync(CreateDesignRequestDto dto);
    Task FinalizeRequestAsync(int requestId, FinalizeRequestDto dto);
    Task<OrderResponseDto> PlaceOrderAsync(CreateOrderDto dto);
}