namespace StarWarsUnlimited.Models;

/// <summary>Аспект (цвет) карты: Vigilance, Villainy, Cunning, Heroism, Aggression, Command.</summary>
public class Aspect
{
    public int Id { get; set; }

    /// <summary>Название аспекта.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Цвет аспекта в HEX-формате.</summary>
    public string? Color { get; set; }

    /// <summary>Подробное описание аспекта.</summary>
    public string? Description { get; set; }

    /// <summary>Порядок отображения в фильтрах.</summary>
    public int SortValue { get; set; }
}
