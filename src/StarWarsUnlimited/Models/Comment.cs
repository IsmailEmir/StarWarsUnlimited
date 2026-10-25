namespace StarWarsUnlimited.Models;

/// <summary>Комментарий пользователя к карточке.</summary>
public class Comment
{
    public int Id { get; set; }

    /// <summary>Идентификатор карточки, к которой относится комментарий.</summary>
    public int CardId { get; set; }

    /// <summary>Имя автора комментария.</summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>Текст комментария.</summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>Дата создания комментария.</summary>
    public DateTime? CreatedAt { get; set; }
}
