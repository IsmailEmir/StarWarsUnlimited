namespace StarWarsUnlimited.Models;

/// <summary>Сет (издание) карт — например, Spark of Rebellion (SOR).</summary>
public class Set
{
    public int Id { get; set; }

    /// <summary>Полное название сета.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Короткий код сета: SOR, SHD, JTL...</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Описание сета.</summary>
    public string? Description { get; set; }

    /// <summary>Дата выхода сета.</summary>
    public DateOnly? ReleaseDate { get; set; }

    /// <summary>Порядок отображения в фильтрах.</summary>
    public int SortValue { get; set; }
}
