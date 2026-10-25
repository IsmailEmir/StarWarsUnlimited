namespace StarWarsUnlimited.Models;

/// <summary>Администратор сайта.</summary>
public class Admin
{
    public int Id { get; set; }

    /// <summary>Логин администратора.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Email администратора.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Хеш пароля (PBKDF2-SHA256, base64).</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Соль для хеширования пароля (base64).</summary>
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>Роль: admin, editor.</summary>
    public string Role { get; set; } = "admin";

    /// <summary>Дата регистрации.</summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>Дата последнего входа.</summary>
    public DateTime? LastLoginAt { get; set; }
}
