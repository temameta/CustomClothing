using CustomClothing.Dto;
using CustomClothing.Service;
using Microsoft.AspNetCore.Mvc;

namespace CustomClothing.Controller;

[ApiController]
[Route("api")]
public class ClothingController(IClothingService clothingService) : ControllerBase
{
    [HttpGet("catalog")]
    public async Task<ActionResult<IEnumerable<CompletedWorkDto>>> GetCatalog()
    {
        var items = await clothingService.GetPublicCatalogAsync();
        return Ok(items);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await clothingService.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpPost("request")]
    public async Task<IActionResult> CreateRequest([FromBody] CreateDesignRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CustomerPhone))
            return BadRequest("Номер телефона обязателен для связи с менеджером.");

        await clothingService.CreateRequestAsync(dto);
        return Ok(new { Message = "Заявка принята. Менеджер свяжется с вами в ближайшее время." });
    }
    
    [HttpPost("request/{id}/finalize")]
    public async Task<IActionResult> FinalizeRequest(int id, [FromBody] FinalizeRequestDto dto)
    {
        try
        {
            await clothingService.FinalizeRequestAsync(id, dto);
            return Ok(new { Message = "Заявка успешно перенесена в выполненные работы." });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("order")]
    public async Task<ActionResult<OrderResponseDto>> PlaceOrder([FromBody] CreateOrderDto dto)
    {
        try
        {
            var result = await clothingService.PlaceOrderAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}