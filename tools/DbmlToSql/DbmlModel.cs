namespace DbmlToSql;

/// <summary>Колонка таблицы, разобранная из DBML.</summary>
public sealed class DbmlColumn
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool PrimaryKey { get; set; }
    public bool NotNull { get; set; }
    public bool Unique { get; set; }
    public bool Increment { get; set; }
    public string? Default { get; set; }
    public string? Note { get; set; }
    public string? Reference { get; set; }
}

/// <summary>Таблица, разобранная из DBML.</summary>
public sealed class DbmlTable
{
    public string Name { get; set; } = string.Empty;
    public string? Alias { get; set; }
    public string? Note { get; set; }
    public List<DbmlColumn> Columns { get; } = new();
    public List<DbmlIndex> Indexes { get; } = new();
}

/// <summary>Связь (Ref) между таблицами.</summary>
public sealed class DbmlRef
{
    public string TableName { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
    public string RefTableName { get; set; } = string.Empty;
    public string RefColumnName { get; set; } = "id";
    public string Relation { get; set; } = ">";
    public string? OnDelete { get; set; }
    public string? OnUpdate { get; set; }
}

/// <summary>Индекс.</summary>
public sealed class DbmlIndex
{
    public string Name { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public List<string> Columns { get; set; } = new();
    public bool Unique { get; set; }
}
