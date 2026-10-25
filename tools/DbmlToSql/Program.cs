using DbmlToSql;

// ============================================================================
// Генератор SQL-скриптов из DBML-схемы.
//
//   dotnet run --project tools/DbmlToSql
//   dotnet run --project tools/DbmlToSql -- <путь к .dbml> <папка вывода>
//
// Источник истины — docs/dbml/schema.dbml. Скрипты генерируются из него,
// поэтому схема в БД и схема в dbdiagram.io всегда совпадают.
// ============================================================================

var repoRoot = FindRepoRoot();
var dbmlPath = args.Length > 0
    ? Path.GetFullPath(args[0])
    : Path.Combine(repoRoot, "docs", "dbml", "schema.dbml");

var outputDir = args.Length > 1
    ? Path.GetFullPath(args[1])
    : Path.Combine(repoRoot, "src", "StarWarsUnlimited", "Database");

Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
Console.WriteLine("║   DBML -> SQL генератор схемы БД                            ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
Console.WriteLine($"  Источник: {dbmlPath}");

if (!File.Exists(dbmlPath))
{
    Console.Error.WriteLine("  ✗ Файл DBML не найден.");
    return 1;
}

var source = File.ReadAllText(dbmlPath);

var parser = new DbmlParser();
parser.Parse(source);

Console.WriteLine($"  Проект:   {parser.ProjectName ?? "(не указан)"}");
Console.WriteLine($"  Таблиц:   {parser.Tables.Count}");
Console.WriteLine($"  Связей:   {parser.Refs.Count}");
Console.WriteLine($"  Индексов: {parser.Indexes.Count}");
Console.WriteLine();

if (parser.Tables.Count == 0)
{
    Console.Error.WriteLine("  ✗ Не удалось разобрать ни одной таблицы — проверьте синтаксис DBML.");
    return 1;
}

Directory.CreateDirectory(outputDir);

var generator = new SqlGenerator(parser);

var schema = generator.GenerateSchema();
var constraints = generator.GenerateConstraints();
var indexes = generator.GenerateIndexes();

var schemaPath = Path.Combine(outputDir, "01_schema.sql");
var constraintsPath = Path.Combine(outputDir, "02_constraints.sql");
var indexesPath = Path.Combine(outputDir, "03_indexes.sql");

File.WriteAllText(schemaPath, schema);
File.WriteAllText(constraintsPath, constraints);
File.WriteAllText(indexesPath, indexes);

Console.WriteLine("  Созданы файлы:");
Console.WriteLine($"    → {schemaPath}");
Console.WriteLine($"    → {constraintsPath}");
Console.WriteLine($"    → {indexesPath}");
Console.WriteLine();

Console.WriteLine("  Таблицы:");
foreach (var t in parser.Tables)
{
    var pk = t.Columns.Where(c => c.PrimaryKey).Select(c => c.Name);
    Console.WriteLine($"    {t.Name,-16} колонок: {t.Columns.Count,2}  PK: {string.Join(",", pk)}");

    var fkList = parser.Refs
        .Where(r => r.TableName == t.Name)
        .Select(r => $"{r.ColumnName}→{r.RefTableName}")
        .ToList();

    if (fkList.Count > 0)
    {
        Console.WriteLine($"    {"",-16} FK: {string.Join(", ", fkList)}");
    }
}

Console.WriteLine();
Console.WriteLine("  ✓ Готово.");
return 0;

// Ищет корень репозитория (где лежит .git или .sln), поднимаясь вверх по дереву.
static string FindRepoRoot()
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);

    while (dir is not null)
    {
        if (Directory.Exists(Path.Combine(dir.FullName, ".git")) ||
            dir.GetFiles("*.sln").Length > 0)
        {
            return dir.FullName;
        }

        dir = dir.Parent;
    }

    return Directory.GetCurrentDirectory();
}
