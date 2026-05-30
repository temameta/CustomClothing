using CustomClothing.dto.request;
using CustomClothing.dto.response;
using CustomClothing.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CustomClothing.api;

public static class ClothingEndpoints
{
    public static void MapClothingEndpoints(this IEndpointRouteBuilder app)
    {
        var customer = app.MapGroup("/api")
            .WithTags("Customer");
        
        customer.MapGet("/catalog", async (IClothingService service) =>
            Results.Ok(await service.GetPublicCatalogAsync()))
            .WithSummary("Просмотр каталога готовых работ")
            .WithDescription("Возвращает список всех изделий, которые мастер пометил как публичные.")
            .Produces<IEnumerable<CompletedWorkDto>>(StatusCodes.Status200OK);
        
        customer.MapPost("/requests/{id}/finalize", async (int id, FinalizeRequestDto dto, IClothingService service) => {
                try
                {
                    await service.FinalizeRequestAsync(id, dto);
                    return Results.Ok(new { message = "Заявка завершена." });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new ErrorResponse(ex.Message));
                }
            })
            .WithSummary("Финализация заявки")
            .WithDescription("Закрывает заявку и переносит в каталог.")
            .Produces(200).Produces<ErrorResponse>(400);

        customer.MapGet("/categories", async (IClothingService service) =>
            Results.Ok(await service.GetCategoriesAsync()))
            .WithSummary("Получить список категорий одежды")
            .Produces<IEnumerable<CategoryDto>>(StatusCodes.Status200OK);
        
        customer.MapPost("/requests", async (CreateDesignRequestDto dto, IClothingService service) =>
        {
            await service.CreateRequestAsync(dto);
            return Results.StatusCode(StatusCodes.Status201Created);
        })
        .WithSummary("Оставить заявку на индивидуальный дизайн")
        .WithDescription("Позволяет пользователю описать свою идею. После этого с ним свяжется менеджер.")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        customer.MapPost("/orders", async (CreateOrderDto dto, IClothingService service) =>
        {
            try
            {
                var response = await service.PlaceOrderAsync(dto);
                return Results.Created($"/api/orders/{response.Id}", response);
            }
            catch (Exception ex)
            {
                return Results.Json(new ErrorResponse(ex.Message), statusCode: 400);
            }
        })
        .WithSummary("Заказать уже готовую вещь из каталога")
        .Produces<OrderResponseDto>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
        
        customer.MapPost("/orders/{id}/cancel", async (int id, IClothingService service) =>
            {
                try
                {
                    await service.CancelOrderAsync(id);
                    return Results.Ok(new { message = "Заказ успешно отменен." });
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.Json(new ErrorResponse(ex.Message), statusCode: 404);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Json(new ErrorResponse(ex.Message), statusCode: 400);
                }
            })
            .WithSummary("Отменить заказ")
            .WithDescription("Позволяет пользователю отменить свой заказ, если он еще не был обработан менеджером.")
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
    }
}