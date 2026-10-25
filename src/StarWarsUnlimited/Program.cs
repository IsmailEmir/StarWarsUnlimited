using StarWarsUnlimited.Configuration;
using StarWarsUnlimited.Core.Http;
using StarWarsUnlimited.Core.Mvc;

// ============================================================================
// Star Wars Unlimited — веб-сервер на базе HttpListener (собственная MVC).
// Запуск: dotnet run --project src/StarWarsUnlimited
// ============================================================================

var config = new AppConfig
{
    ServerUrl = "http://localhost:5000",
    DbHost = "localhost",
    DbPort = 5432,
    DbName = "starwarsunlimited",
    DbUser = "postgres",
    DbPassword = ""
};

// Создаём и запускаем сервер
using var server = new HttpServer(config.ServerUrl, config.DbHost, config.DbPort, config.DbName, config.DbUser, config.DbPassword);
await server.StartAsync();

Console.WriteLine("  Нажмите Ctrl+C для остановки сервера.");

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

await Task.Delay(Timeout.Infinite, cts.Token).ContinueWith(_ => { }, TaskContinuationOptions.ExecuteSynchronously);
await server.StopAsync();

Console.WriteLine("  Сервер остановлен.");
