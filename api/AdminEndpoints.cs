using CustomClothing.dto.request;
using CustomClothing.dto.response;
using CustomClothing.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CustomClothing.api;

public static class AdminEndpoints {
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app) {
        var admin = app.MapGroup("/api/admin")
            .WithTags("Admin");
        
        admin.MapGet("/requests", async (IAdminService service) => 
            Results.Ok(await service.GetAllRequestsAsync()))
            .WithSummary("Получить все заявки")
            .Produces<IEnumerable<DesignRequestResponse>>(200);

        admin.MapGet("/requests/{id}", async (int id, IAdminService service) => {
            var res = await service.GetRequestByIdAsync(id);
            return res != null ? Results.Ok(res) : Results.NotFound(new ErrorResponse("Заявка не найдена"));
        })
        .WithSummary("Получить заявку по ID")
        .Produces<DesignRequestResponse>(200).Produces<ErrorResponse>(404);

        admin.MapDelete("/requests/{id}", async (int id, IAdminService service) => {
            await service.DeleteRequestAsync(id); return Results.NoContent();
        })
        .WithSummary("Удалить заявку").Produces(204);
        
        admin.MapGet("/orders", async (IAdminService service) => 
            Results.Ok(await service.GetAllOrdersAsync()))
            .WithSummary("Получить все заказы")
            .Produces<IEnumerable<OrderAdminResponse>>(200);

        admin.MapGet("/orders/{id}", async (int id, IAdminService service) => {
            var res = await service.GetOrderByIdAsync(id);
            return res != null ? Results.Ok(res) : Results.NotFound(new ErrorResponse("Заказ не найден"));
        })
        .WithSummary("Получить заказ по ID")
        .Produces<OrderAdminResponse>(200).Produces<ErrorResponse>(404);

        admin.MapDelete("/orders/{id}", async (int id, IAdminService service) => {
            await service.DeleteOrderAsync(id); return Results.NoContent();
        })
        .WithSummary("Удалить заказ").Produces(204);
        admin.MapPost("/categories", async (CreateCategoryDto dto, IAdminService service) =>
            {
                try
                {
                    var result = await service.CreateCategoryAsync(dto);
                    return Results.Created($"/api/categories", result);
                }
                catch (ArgumentException ex)
                {
                    return Results.Json(new ErrorResponse(ex.Message), statusCode: 400);
                }
            })
            .WithSummary("Добавить новую категорию одежды")
            .WithDescription("Позволяет менеджеру расширить список доступных типов одежды (например, добавить 'Свитшоты').")
            .Produces<CategoryDto>(StatusCodes.Status201Created)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);
    }
}