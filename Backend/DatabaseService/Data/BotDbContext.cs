// DatabaseService/Services/BotDbContext.cs

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Контекст данных приложения.
/// </summary>
public class BotDbContext : DbContext
{
    public BotDbContext()
    {
        
    }
    /// <summary>
    /// Настраиваемый конструктор.
    /// </summary>
    /// <param name="options">Опции.</param>
    public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Таблица активностей.
    /// </summary>
    public DbSet<Activity> Activities { get; set; }

    /// <summary>
    /// Бронирвания.
    /// </summary>
    public DbSet<Booking> Bookings { get; set; }

    /// <summary>
    /// Пользователи.
    /// </summary>
    public DbSet<UserProfile> UserProfiles { get; set; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite();
    }
}