// DatabaseService/Repositories/Activity.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Активность.
/// </summary>
public class Activity
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Название активности.
    /// </summary>
    [FillableField("Название активности:")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Дата начала активности.
    /// </summary>
    [FillableField("Дата начала:")]
    public string? StartDateString { get; set; }

    /// <summary>
    /// фасилитатор. Ответственный за активность.
    /// </summary>
    public UserProfile? Facilitator { get; set; }

    /// <summary>
    /// Описание активности.
    /// </summary>
    [FillableField("Описание мероприятия:")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Перечень инвентаря.
    /// </summary>
     [FillableField("Перечень инвентаря:")]
    public string Inventory { get; set; } = string.Empty;

    /// <summary>
    /// Максимальное количество участников.
    /// </summary>
    [FillableField("Максимальное количество участников:")]
    public int MaxParticipants { get; set; }

    /// <summary>
    /// Путь до картинки баннера.
    /// </summary>
    /// todo: 
    public string? Banner { get; set; }

    /// <summary>
    /// Забронированые места.
    /// </summary>
    public List<Booking> Bookings { get; set; } = [];
    
    [NotMapped]
    private DateTime StartDate { get; set; }
}
