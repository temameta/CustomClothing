using CustomClothing.dto.request;
using CustomClothing.dto.response;

namespace CustomClothing.interfaces;

public interface IAdminService
{
    Task<IEnumerable<DesignRequestResponse>> GetAllRequestsAsync();
    Task<DesignRequestResponse?> GetRequestByIdAsync(int id);
    Task DeleteRequestAsync(int id);
    Task<IEnumerable<OrderAdminResponse>> GetAllOrdersAsync();
    Task<OrderAdminResponse?> GetOrderByIdAsync(int id);
    Task DeleteOrderAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
}