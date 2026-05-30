using CustomClothing.Dto;
using CustomClothing.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomClothing.Controller;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class ClothingController(IClothingService clothingService) : ControllerBase
{
    /// <summary>
    /// Получить каталог публичных работ.
    /// </summary>
    /// <remarks>
    /// Возвращает только те работы, которые помечены в базе как "публичные".
    /// </remarks>
    /// <response code="200">Возвращает список выполненных работ.</response>
    [HttpGet("catalog")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CompletedWorkDto>>> GetCatalog()
    {
        var items = await clothingService.GetPublicCatalogAsync();
        return Ok(items);
    }

    /// <summary>
    /// Получить список доступных категорий одежды.
    /// </summary>
    /// <response code="200">Возвращает ID и названия категорий (Куртки, Футболки и т.д.).</response>
    [HttpGet("categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await clothingService.GetCategoriesAsync();
        return Ok(categories);
    }

    /// <summary>
    /// Оставить заявку на создание собственного кастомного дизайна.
    /// </summary>
    /// <param name="dto">Данные заявки: название, описание, контактные данные клиента.</param>
    /// <response code="200">Заявка успешно принята.</response>
    /// <response code="400">Ошибка валидации данных (например, не указан телефон).</response>
    [HttpPost("request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRequest([FromBody] CreateDesignRequestDto dto)
    {
        await clothingService.CreateRequestAsync(dto);
        return Ok(new { Message = "Заявка принята. Менеджер свяжется с вами в ближайшее время." });
    }

    /// <summary>
    /// Заказать уже существующую вещь из каталога.
    /// </summary>
    /// <param name="dto">ID работы из каталога и данные для доставки.</param>
    /// <response code="200">Заказ успешно создан.</response>
    /// <response code="404">Указанная работа не найдена в каталоге.</response>
    [HttpPost("order")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponseDto>> PlaceOrder([FromBody] CreateOrderDto dto)
    {
        try
        {
            var result = await clothingService.PlaceOrderAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Завершить работу по заявке (Действие менеджера).
    /// </summary>
    /// <remarks>
    /// После вызова этого метода:
    /// 1. Статус заявки меняется на "Завершено".
    /// 2. Работа добавляется в каталог (публично или скрыто).
    /// 3. Автоматически создается оплаченный заказ на эту работу.
    /// </remarks>
    /// <param name="id">ID заявки на дизайн.</param>
    /// <param name="dto">Настройки публикации в каталог.</param>
    /// <response code="200">Заявка успешно финализирована и перенесена в заказы.</response>
    /// <response code="400">Заявка уже была завершена или данные некорректны.</response>
    [HttpPost("request/{id}/finalize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> FinalizeRequest(int id, [FromBody] FinalizeRequestDto dto)
    {
        try
        {
            await clothingService.FinalizeRequestAsync(id, dto);
            return Ok(new { Message = "Заявка успешно перенесена в выполненные работы и заказы." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}