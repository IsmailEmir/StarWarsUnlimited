using System.Text;
using System.Text.RegularExpressions;

namespace DbmlToSql;

/// <summary>
/// Разбирает DBML-файл (dbdiagram.io) в объектную модель схемы.
/// Поддерживает: Project, Table, Column, настройки колонок, Note, Ref, Indexes.
/// </summary>
public sealed class DbmlParser
{
    public List<DbmlTable> Tables { get; } = new();
    public List<DbmlRef> Refs { get; } = new();
    public List<DbmlIndex> Indexes { get; } = new();
    public string? ProjectName { get; private set; }
    public string DatabaseType { get; private set; } = "PostgreSQL";

    public void Parse(string source)
    {
        var lines = source.Replace("\r\n", "\n").Split('\n');
        int i = 0;

        while (i < lines.Length)
        {
            var line = lines[i].Trim();

            if (line.Length == 0 || line.StartsWith("//"))
            {
                i++;
                continue;
            }

            if (line.StartsWith("Table ", StringComparison.Ordinal))
            {
                i = ParseTable(lines, i);
                continue;
            }

            if (line.StartsWith("Ref:", StringComparison.Ordinal) ||
                line.StartsWith("Ref ", StringComparison.Ordinal))
            {
                i = ParseRef(lines, i);
                continue;
            }

            if (line.StartsWith("Project ", StringComparison.Ordinal))
            {
                i = ParseProject(lines, i);
                continue;
            }

            if (line.StartsWith("Index ", StringComparison.Ordinal))
            {
                i = ParseIndex(lines, i);
                continue;
            }

            i++;
        }
    }

    private int ParseProject(string[] lines, int start)
    {
        var header = lines[start].Trim();
        var body = new List<string>();
        int i = start + 1;

        while (i < lines.Length && lines[i].Trim() != "}")
        {
            body.Add(lines[i]);
            i++;
        }

        i++;

        var m = Regex.Match(header, @"^Project\s+([\w\.]+)");
        if (m.Success)
        {
            ProjectName = m.Groups[1].Value;
        }

        foreach (var b in body)
        {
            var dm = Regex.Match(b.Trim(), @"^database_type:\s*'([^']+)'", RegexOptions.IgnoreCase);
            if (dm.Success)
            {
                DatabaseType = dm.Groups[1].Value;
            }
        }

        return i;
    }

    private int ParseTable(string[] lines, int start)
    {
        var header = lines[start].Trim();
        var nameMatch = Regex.Match(header, @"^Table\s+([\w\.]+)(?:\s+as\s+(\w+))?");
        if (!nameMatch.Success)
        {
            return start + 1;
        }

        var table = new DbmlTable
        {
            Name = nameMatch.Groups[1].Value,
            Alias = nameMatch.Groups[2].Success ? nameMatch.Groups[2].Value : null
        };

        int i = start + 1;
        while (i < lines.Length)
        {
            var line = lines[i].Trim();

            if (line == "}")
            {
                i++;
                break;
            }

            if (line.Length == 0 || line.StartsWith("//"))
            {
                i++;
                continue;
            }

            var noteMatch = Regex.Match(line, @"^Note:\s*'(.*)'$");
            if (noteMatch.Success)
            {
                table.Note = noteMatch.Groups[1].Value.Replace("''", "'");
                i++;
                continue;
            }

            if (line.StartsWith("Indexes", StringComparison.Ordinal))
            {
                i++;
                while (i < lines.Length && lines[i].Trim() != "}")
                {
                    var idxLine = lines[i].Trim();
                    if (idxLine.Length > 0 && !idxLine.StartsWith("//"))
                    {
                        var idx = ParseIndexLine(idxLine, table.Name);
                        if (idx != null)
                        {
                            table.Indexes.Add(idx);
                            Indexes.Add(idx);
                        }
                    }
                    i++;
                }
                i++;
                continue;
            }

            var col = ParseColumn(line);
            if (col != null)
            {
                table.Columns.Add(col);
            }

            i++;
        }

        Tables.Add(table);
        return i;
    }

    private static DbmlColumn? ParseColumn(string line)
    {
        var withoutComment = line;
        var commentIdx = withoutComment.IndexOf("/*", StringComparison.Ordinal);
        if (commentIdx >= 0)
        {
            withoutComment = withoutComment[..commentIdx].Trim();
        }

        var m = Regex.Match(withoutComment, @"^([\w]+)\s+([\w\(\)\s]+?)(?:\s*\[(.*)\])?$");
        if (!m.Success)
        {
            return null;
        }

        var name = m.Groups[1].Value;
        var type = m.Groups[2].Value.Trim();

        if (type.Contains(' '))
        {
            var quoted = Regex.Match(type, "\"(.+)\"");
            type = quoted.Success ? quoted.Groups[1].Value : type.Split(' ')[0];
        }

        var column = new DbmlColumn { Name = name, Type = type };

        var settings = m.Groups[3].Success ? m.Groups[3].Value : string.Empty;
        if (settings.Length > 0)
        {
            ApplySettings(column, settings);
        }

        return column;
    }

    private static void ApplySettings(DbmlColumn column, string settings)
    {
        var parts = new List<string>();
        var current = new StringBuilder();
        bool inQuote = false;

        foreach (var ch in settings)
        {
            if (ch == '\'')
            {
                inQuote = !inQuote;
                current.Append(ch);
                continue;
            }

            if (ch == ',' && !inQuote)
            {
                parts.Add(current.ToString());
                current.Clear();
                continue;
            }

            current.Append(ch);
        }

        if (current.Length > 0)
        {
            parts.Add(current.ToString());
        }

        foreach (var raw in parts)
        {
            var part = raw.Trim();
            if (part.Length == 0)
            {
                continue;
            }

            if (part.Equals("pk", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("primary key", StringComparison.OrdinalIgnoreCase))
            {
                column.PrimaryKey = true;
            }
            else if (part.Equals("not null", StringComparison.OrdinalIgnoreCase))
            {
                column.NotNull = true;
            }
            else if (part.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                column.NotNull = false;
            }
            else if (part.Equals("unique", StringComparison.OrdinalIgnoreCase))
            {
                column.Unique = true;
            }
            else if (part.Equals("increment", StringComparison.OrdinalIgnoreCase))
            {
                column.Increment = true;
            }
            else if (part.StartsWith("default:", StringComparison.OrdinalIgnoreCase))
            {
                column.Default = part["default:".Length..].Trim();
            }
            else if (part.StartsWith("note:", StringComparison.OrdinalIgnoreCase))
            {
                var note = part["note:".Length..].Trim();
                if (note.Length >= 2 && note[0] == '\'' && note[^1] == '\'')
                {
                    note = note[1..^1];
                }
                column.Note = note.Replace("''", "'");
            }
            else if (part.StartsWith("ref:", StringComparison.OrdinalIgnoreCase))
            {
                column.Reference = part["ref:".Length..].Trim();
            }
        }
    }

    private int ParseRef(string[] lines, int start)
    {
        var line = lines[start].Trim();

        while (!line.Contains('>') && !line.Contains('<') && !line.Contains('-') &&
               start + 1 < lines.Length && !lines[start + 1].Trim().StartsWith("//"))
        {
            start++;
            line += " " + lines[start].Trim();
        }

        var m = Regex.Match(line, @"^Ref:\s*([\w\.]+)\s*(<|>|<>|-|\?)\s*([\w\.]+)(?:\s*\[(.*)\])?$");
        if (!m.Success)
        {
            return start + 1;
        }

        var leftParts = m.Groups[1].Value.Split('.');
        var rightParts = m.Groups[3].Value.Split('.');

        var dbmlRef = new DbmlRef
        {
            TableName = leftParts[0],
            ColumnName = leftParts.Length > 1 ? leftParts[1] : string.Empty,
            RefTableName = rightParts[0],
            RefColumnName = rightParts.Length > 1 ? rightParts[1] : "id",
            Relation = m.Groups[2].Value
        };

        var settings = m.Groups[4].Success ? m.Groups[4].Value : string.Empty;
        foreach (var s in settings.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var kv = s.Split(':', 2);
            if (kv.Length == 2)
            {
                var key = kv[0].Trim().ToLowerInvariant();
                var value = kv[1].Trim().ToLowerInvariant();
                if (key == "delete")
                {
                    dbmlRef.OnDelete = value;
                }
                else if (key == "update")
                {
                    dbmlRef.OnUpdate = value;
                }
            }
        }

        Refs.Add(dbmlRef);
        return start + 1;
    }

    private int ParseIndex(string[] lines, int start)
    {
        var header = lines[start].Trim();
        var m = Regex.Match(header, @"^Index\s+([\w\.]+)");
        if (!m.Success)
        {
            return start + 1;
        }

        var name = m.Groups[1].Value;
        int i = start + 1;
        while (i < lines.Length && lines[i].Trim() != "}")
        {
            var line = lines[i].Trim();
            if (line.Length > 0 && !line.StartsWith("//"))
            {
                var idx = ParseIndexLine(line, null, name);
                if (idx != null)
                {
                    Indexes.Add(idx);
                }
            }
            i++;
        }

        return i + 1;
    }

    private static DbmlIndex? ParseIndexLine(string line, string? tableName, string? explicitName = null)
    {
        var m = Regex.Match(line, @"^(?:([\w\.]+)\s+)?\(([^)]+)\)(?:\s*\[(.*)\])?$");
        if (!m.Success)
        {
            return null;
        }

        var columns = m.Groups[2].Value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(c => c.Split(' ')[0].Trim())
            .Where(c => c.Length > 0)
            .ToList();

        if (columns.Count == 0)
        {
            return null;
        }

        var index = new DbmlIndex
        {
            TableName = tableName ?? m.Groups[1].Value,
            Name = explicitName ?? m.Groups[1].Value,
            Columns = columns
        };

        var settings = m.Groups[3].Success ? m.Groups[3].Value : string.Empty;
        foreach (var s in settings.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (s.Equals("unique", StringComparison.OrdinalIgnoreCase))
            {
                index.Unique = true;
            }
        }

        if (string.IsNullOrEmpty(index.Name))
        {
            index.Name = string.Join("_", index.TableName.Split('.').Concat(columns));
        }

        return index;
    }
}
