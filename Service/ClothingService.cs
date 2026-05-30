using CustomClothing.Data;
using CustomClothing.Dto;
using CustomClothing.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomClothing.Service;

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
            CustomerPhone = dto.CustomerPhone,
            ClothingCategoryId = dto.CategoryId
        };

        context.DesignRequests.Add(request);
        await context.SaveChangesAsync();
    }
    
    public async Task FinalizeRequestAsync(int requestId, FinalizeRequestDto dto)
    {
        var request = await context.DesignRequests
            .Include(r => r.Category)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new Exception("Заявка не найдена");
        if (request.Status == RequestStatus.Completed) throw new Exception("Заявка уже была завершена");
        
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
    }
}