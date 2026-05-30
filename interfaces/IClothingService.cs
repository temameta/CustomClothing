using CustomClothing.dto.request;
using CustomClothing.dto.response;

namespace CustomClothing.interfaces;

public interface IClothingService
{
    Task<IEnumerable<CompletedWorkDto>> GetPublicCatalogAsync();
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task CreateRequestAsync(CreateDesignRequestDto dto);
    Task FinalizeRequestAsync(int requestId, FinalizeRequestDto dto);
    Task<OrderResponseDto> PlaceOrderAsync(CreateOrderDto dto);
    Task DeleteRequestAsync(int id);
    Task<DesignRequestResponse?> GetRequestByIdAsync(int id); 
}