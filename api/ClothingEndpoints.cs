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
        var api = app.MapGroup("/api")
            .WithOpenApi();
        
        api.MapGet("/catalog", async (IClothingService service) =>
            Results.Ok(await service.GetPublicCatalogAsync()))
            .WithSummary("Получить каталог публичных работ")
            .WithDescription("Возвращает список всех выполненных работ, помеченных как публичные.")
            .Produces<IEnumerable<CompletedWorkDto>>(StatusCodes.Status200OK);
        
        api.MapGet("/categories", async (IClothingService service) =>
            Results.Ok(await service.GetCategoriesAsync()))
            .WithSummary("Получить список категорий")
            .Produces<IEnumerable<CategoryDto>>(StatusCodes.Status200OK);
        
        api.MapGet("/requests/{id}", async (int id, IClothingService service) =>
        {
            var res = await service.GetRequestByIdAsync(id);
            return res is not null 
                ? Results.Ok(res) 
                : Results.Json(new ErrorResponse("Заявка не найдена"), statusCode: 404);
        })
        .WithSummary("Получить заявку по ID")
        .Produces<DesignRequestResponse>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        api.MapPost("/requests", async (CreateDesignRequestDto dto, IClothingService service) =>
        {
            await service.CreateRequestAsync(dto);
            return Results.Created($"/api/requests", dto);
        })
        .WithSummary("Создать новую заявку на дизайн")
        .WithDescription("Клиент оставляет пожелания к будущей одежде.")
        .Produces(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
        
        api.MapPost("/orders", async (CreateOrderDto dto, IClothingService service) =>
        {
            try
            {
                var response = await service.PlaceOrderAsync(dto);
                return Results.Created($"/api/orders/{response.Id}", response);
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
        .WithSummary("Заказать готовую вещь")
        .WithDescription("Оформление заказа на товар, который уже есть в каталоге.")
        .Produces<OrderResponseDto>(StatusCodes.Status201Created)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
        
        api.MapPost("/requests/{id}/finalize", async (int id, FinalizeRequestDto dto, IClothingService service) =>
        {
            try
            {
                await service.FinalizeRequestAsync(id, dto);
                return Results.Ok(new { message = "Заявка завершена, создана работа и заказ." });
            }
            catch (KeyNotFoundException)
            {
                return Results.Json(new ErrorResponse("Заявка не найдена"), statusCode: 404);
            }
            catch (Exception ex)
            {
                return Results.Json(new ErrorResponse(ex.Message), statusCode: 400);
            }
        })
        .WithSummary("Финализировать заявку")
        .WithDescription("Сквозная операция: закрывает заявку, добавляет её в каталог и создает заказ.")
        .Produces(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
        
        api.MapDelete("/requests/{id}", async (int id, IClothingService service) =>
        {
            await service.DeleteRequestAsync(id);
            return Results.NoContent();
        })
        .WithSummary("Удалить заявку")
        .Produces(StatusCodes.Status204NoContent);
    }
}