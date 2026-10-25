using System.Net;

namespace StarWarsUnlimited.Configuration;

/// <summary>
/// Конфигурация приложения, загруженная из appsettings.json.
/// </summary>
public static class AppConfig
{
    public static string ServerUrl { get; set; } = "http://localhost:5000";
    public static string ApiBaseUrl { get; set; } = "https://admin.starwarsunlimited.com/api";

    // PostgreSQL конфиг
    public static string DbHost { get; set; } = "localhost";
    public static int DbPort { get; set; } = 5432;
    public static string DbName { get; set; } = "starwarsunlimited";
    public static string DbUser { get; set; } = "postgres";
    public static string DbPassword { get; set; } = "";

    public static void Load()
    {
        // В будущем читаем из appsettings.json, а пока используем дефолты
        // можно переопределять через переменные окружения
    }
}