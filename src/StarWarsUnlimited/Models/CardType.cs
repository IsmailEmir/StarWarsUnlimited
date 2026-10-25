namespace StarWarsUnlimited.Models;

/// <summary>Тип карты: Leader, Unit, Event, Base, Upgrade, Token.</summary>
public class CardType
{
    public int Id { get; set; }

    /// <summary>Название типа.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Значение типа из API.</summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>Порядок отображения в фильтрах.</summary>
    public int SortValue { get; set; }
}
