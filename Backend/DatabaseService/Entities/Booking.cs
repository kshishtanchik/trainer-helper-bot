// DatabaseService/Services/Booking.cs

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Бронирование.
/// </summary>
public class Booking
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    required public UserProfile User { get; set; }

    /// <summary>
    /// Активность.
    /// </summary>
    required public Activity Activity { get; set; }

    /// <summary>
    /// Присутствовал.
    /// </summary>
    public bool Present { get; set; }=false;
}