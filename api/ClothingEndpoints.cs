using CustomClothing.dto.request;
using CustomClothing.dto.response;
using CustomClothing.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CustomClothing.api;

public static class ClothingEndpoints {
    public static void MapClothingEndpoints(this IEndpointRouteBuilder app) {
        var api = app.MapGroup("/api");

        // 1. Получение категорий
        api.MapGet("/categories", async (IClothingService service) => 
            Results.Ok(await service.GetCategoriesAsync()));

        // 2. Оформление заказа на готовую вещь
        api.MapPost("/orders", async (CreateOrderDto dto, IClothingService service) => {
            try {
                var response = await service.PlaceOrderAsync(dto);
                // Согласно ТЗ: Возвращаем 201 Created
                return Results.Created($"/api/orders/{response.Id}", response);
            } 
            catch (KeyNotFoundException ex) {
                return Results.Json(new ErrorResponse(ex.Message), statusCode: 404);
            }
            catch (InvalidOperationException ex) {
                return Results.Json(new ErrorResponse(ex.Message), statusCode: 400);
            }
        });

        // 3. Каталог
        api.MapGet("/catalog", async (IClothingService service) => 
            Results.Ok(await service.GetPublicCatalogAsync()));

        // 4. Заявки (создание)
        api.MapPost("/requests", async (CreateDesignRequestDto dto, IClothingService service) => {
            await service.CreateRequestAsync(dto);
            return Results.StatusCode(201); // 201 Created
        });

        // 5. Заявки (получение по ID)
        api.MapGet("/requests/{id}", async (int id, IClothingService service) => {
            var res = await service.GetRequestByIdAsync(id);
            return res is not null 
                ? Results.Ok(res) 
                : Results.Json(new ErrorResponse("Заявка не найдена"), statusCode: 404);
        });

        // 6. Финализация (Бизнес-операция)
        api.MapPost("/requests/{id}/finalize", async (int id, FinalizeRequestDto dto, IClothingService service) => {
            try {
                await service.FinalizeRequestAsync(id, dto);
                return Results.Ok(new { message = "Заявка завершена, создана работа в портфолио и новый заказ." });
            } catch (KeyNotFoundException) {
                return Results.Json(new ErrorResponse("Заявка не найдена"), statusCode: 404);
            } catch (Exception ex) {
                return Results.Json(new ErrorResponse(ex.Message), statusCode: 400);
            }
        });

        // 7. Удаление
        api.MapDelete("/requests/{id}", async (int id, IClothingService service) => {
            await service.DeleteRequestAsync(id);
            return Results.NoContent(); // 204 No Content
        });
    }
}