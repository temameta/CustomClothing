using CustomClothing.database;
using CustomClothing.dto.request;
using CustomClothing.dto.response;
using CustomClothing.interfaces;
using CustomClothing.model;
using Microsoft.EntityFrameworkCore;

namespace CustomClothing.services;

public class ClothingService(AppDbContext context) : IClothingService
{
    public async Task<IEnumerable<CompletedWorkDto>> GetPublicCatalogAsync()
    {
        return await context.CompletedWorks
            .Where(w => w.IsPublic)
            .Include(w => w.Category)
            .Select(w => new CompletedWorkDto(w.Id, w.Name, w.Description, w.Category!.Name))
            .ToListAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        return await context.Categories
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync();
    }

    public async Task CreateRequestAsync(CreateDesignRequestDto dto)
    {
        var request = new DesignRequest
        {
            Title = dto.Title,
            Description = dto.Description,
            CustomerName = dto.CustomerName,
            CustomerPhone = dto.CustomerPhone,
            DeliveryAddress = dto.DeliveryAddress,
            ClothingCategoryId = dto.CategoryId
        };

        context.DesignRequests.Add(request);
        await context.SaveChangesAsync();
    }
    
    public async Task FinalizeRequestAsync(int requestId, FinalizeRequestDto dto)
    {
        var request = await context.DesignRequests
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new Exception("Заявка не найдена");
        if (request.Status == RequestStatus.Completed) throw new Exception("Заявка уже завершена");
        
        request.Status = RequestStatus.Completed;
        
        var completedWork = new CompletedWork
        {
            Name = dto.OverrideName ?? request.Title,
            Description = request.Description,
            IsPublic = dto.IsPublic,
            ClothingCategoryId = request.ClothingCategoryId
        };
        context.CompletedWorks.Add(completedWork);
        
        await context.SaveChangesAsync();
        
        var order = new Order
        {
            CompletedWorkId = completedWork.Id,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            DeliveryAddress = request.DeliveryAddress,
            Status = OrderStatus.Paid,
            OrderDate = DateTime.UtcNow
        };
        context.Orders.Add(order);

        await context.SaveChangesAsync();
    }
    
    public async Task<OrderResponseDto> PlaceOrderAsync(CreateOrderDto dto)
    {
        var work = await context.CompletedWorks
            .FirstOrDefaultAsync(w => w.Id == dto.CompletedWorkId);

        if (work == null) 
            throw new Exception("Предмет одежды не найден в каталоге.");

        if (!work.IsPublic)
            throw new Exception("Этот предмет недоступен для заказа.");

        var order = new Order
        {
            CompletedWorkId = dto.CompletedWorkId,
            CustomerName = dto.CustomerName,
            CustomerPhone = dto.CustomerPhone,
            DeliveryAddress = dto.DeliveryAddress,
            Status = OrderStatus.New
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        return new OrderResponseDto(
            order.Id, 
            work.Name, 
            order.Status.ToString(), 
            order.OrderDate
        );
    }
    
    public async Task CancelOrderAsync(int orderId)
    {
        var order = await context.Orders.FindAsync(orderId);

        if (order == null)
            throw new KeyNotFoundException("Заказ не найден.");
        
        if (order.Status != OrderStatus.New)
            throw new InvalidOperationException("Нельзя отменить заказ, который уже обработан или отменен.");

        order.Status = OrderStatus.Cancelled;
        await context.SaveChangesAsync();
    }
}