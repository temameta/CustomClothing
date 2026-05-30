using Microsoft.EntityFrameworkCore;
using CustomClothing.model;

namespace CustomClothing.database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ClothingCategory> Categories => Set<ClothingCategory>();
    public DbSet<CompletedWork> CompletedWorks => Set<CompletedWork>();
    public DbSet<DesignRequest> DesignRequests => Set<DesignRequest>();
    public DbSet<Order> Orders => Set<Order>();

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClothingCategory>().HasData(
            new ClothingCategory { Id = 1, Name = "Куртки" },
            new ClothingCategory { Id = 2, Name = "Футболки" },
            new ClothingCategory { Id = 3, Name = "Штаны" }
        );
        
        modelBuilder.Entity<CompletedWork>().HasData(
            new CompletedWork 
            { 
                Id = 1, 
                Name = "Кожанка 'Old School'", 
                Description = "Классическая куртка с ручной росписью на спине", 
                IsPublic = true, 
                ClothingCategoryId = 1 
            },
            new CompletedWork 
            { 
                Id = 2, 
                Name = "Футболка 'Cyber-Punk'", 
                Description = "Светящийся принт, оверсайз крой", 
                IsPublic = true, 
                ClothingCategoryId = 2 
            },
            new CompletedWork 
            { 
                Id = 3, 
                Name = "Секретный проект X", 
                Description = "Этот заказ не виден обычным пользователям", 
                IsPublic = false,
                ClothingCategoryId = 1 
            }
        );
        
        modelBuilder.Entity<DesignRequest>().HasData(
            new DesignRequest 
            { 
                Id = 1, 
                Title = "Худи с драконом", 
                Description = "Хочу вышивку золотого дракона", 
                CustomerName = "Алексей",
                CustomerPhone = "+79001112233", 
                DeliveryAddress = "ул. Ленина 1",
                ClothingCategoryId = 1,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) 
            },
            new DesignRequest 
            { 
                Id = 2, 
                Title = "Шорты для бега", 
                Description = "Нужны кастомные нашивки по бокам", 
                CustomerName = "Алена",
                CustomerPhone = "88005553535", 
                DeliveryAddress = "ул. Карла Маркса 2",
                ClothingCategoryId = 3,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}