namespace StarWarsUnlimited.Models;

/// <summary>Редкость карты: Common, Uncommon, Rare, Legendary, Special.</summary>
public class Rarity
{
    public int Id { get; set; }

    /// <summary>Название редкости.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Буквенный символ редкости (C, U, R, L).</summary>
    public string Character { get; set; } = string.Empty;

    /// <summary>Цвет рамки карты в HEX-формате.</summary>
    public string? Color { get; set; }

    /// <summary>Порядок отображения в фильтрах.</summary>
    public int SortValue { get; set; }
}
