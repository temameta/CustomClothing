using CustomClothing.database;
using CustomClothing.dto.response;
using CustomClothing.interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomClothing.services;

public class AdminService(AppDbContext context) : IAdminService
{
    public async Task<IEnumerable<DesignRequestResponse>> GetAllRequestsAsync() =>
        await context.DesignRequests.Include(r => r.Category)
            .Select(r => new DesignRequestResponse(r.Id, r.Title, r.Description, r.CustomerName, r.CustomerPhone,
                r.DeliveryAddress, r.Category!.Name, r.Status.ToString()))
            .ToListAsync();

    public async Task<DesignRequestResponse?> GetRequestByIdAsync(int id)
    {
        var r = await context.DesignRequests.Include(r => r.Category).FirstOrDefaultAsync(x => x.Id == id);
        return r == null
            ? null
            : new DesignRequestResponse(r.Id, r.Title, r.Description, r.CustomerName, r.CustomerPhone,
                r.DeliveryAddress, r.Category!.Name, r.Status.ToString());
    }

    public async Task DeleteRequestAsync(int id)
    {
        var r = await context.DesignRequests.FindAsync(id);
        if (r != null)
        {
            context.DesignRequests.Remove(r);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<OrderAdminResponse>> GetAllOrdersAsync() =>
        await context.Orders.Include(o => o.CompletedWork)
            .Select(o => new OrderAdminResponse(o.Id, o.CompletedWork!.Name, o.CustomerName, o.CustomerPhone,
                o.DeliveryAddress, o.OrderDate, o.Status.ToString()))
            .ToListAsync();

    public async Task<OrderAdminResponse?> GetOrderByIdAsync(int id)
    {
        var o = await context.Orders.Include(o => o.CompletedWork).FirstOrDefaultAsync(x => x.Id == id);
        return o == null
            ? null
            : new OrderAdminResponse(o.Id, o.CompletedWork!.Name, o.CustomerName, o.CustomerPhone, o.DeliveryAddress,
                o.OrderDate, o.Status.ToString());
    }

    public async Task DeleteOrderAsync(int id)
    {
        var o = await context.Orders.FindAsync(id);
        if (o != null)
        {
            context.Orders.Remove(o);
            await context.SaveChangesAsync();
        }
    }
}