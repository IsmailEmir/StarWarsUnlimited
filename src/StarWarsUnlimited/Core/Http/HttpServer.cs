namespace StarWarsUnlimited.Core.Http;

using System.Net;

/// <summary>
/// Minimal HTTP server using HttpListener.
/// </summary>
public class HttpServer : IDisposable
{
    private readonly HttpListener _listener;
    private readonly RequestRouter _router;

    public HttpServer()
    {
        _listener = new HttpListener();
        _router = new RequestRouter();
        // Register basic routes
        _router.Map("/", ctx => HomeController.Index(ctx));
        _router.Map("/cards/{id:\\d+}", ctx => CardsController.Show(ctx, int.Parse(ctx.RouteValues["id"]!)));
        _router.Map("/admin", ctx => AdminController.Index(ctx));
        _router.Map("/api/cards/search", ctx => ApiController.Search(ctx));
        _router.Map("/api/cards", ctx => ApiController.List(ctx));
        _router.Map("/admin/login", ctx => AdminController.Login(ctx));
        _router.Map("/admin/logout", ctx => AdminController.Logout(ctx));
    }

    public async Task StartAsync(string url)
    {
        _listener.Prefixes.Add(url);
        _listener.Start;
        Console.WriteLine($"  HTTP server running on {url}");
        _ = ListenLoopAsync();
    }

    public async Task StopAsync()
    {
        _listener.Stop();
        _listener.Close();
    }

    private async Task ListenLoopAsync()
    {
        while (_listener.IsListening)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                _ = ProcessContextAsync(context);
            }
            catch (ObjectDisposedException) { return; }
            catch (Exception ex) { Console.WriteLine($"  Listen error: {ex.Message}"); }
        }
    }

    private async Task ProcessContextAsync(HttpListenerContext context)
    {
        try
        {
            var request = context.Request;
            var response = context.Response;
            string? body = null;

            if (request.HttpMethod == "POST" && request.HasEntityBody)
            {
                using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                body = await reader.ReadToEndAsync();
            }

            var result = _router.Dispatch(request.HttpMethod, request.Url!.PathAndQuery, body);

            if (result == null)
            {
                response.StatusCode = 404;
                response.StatusDescription = "Not Found";
                response.ContentType = "text/html";
                await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("<h1>404 Not Found</h1>"));
                response.Close();
                return;
            }

            response.ContentType = "text/html; charset=utf-8";
            var (statusCode, content) = result;
            response.StatusCode = statusCode;
            await response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes(content));
            response.OutputStream.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ERROR processing request: {ex}");
            try
            {
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = "Internal Server Error";
                await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("<h1>500 Internal Server Error</h1>"));
                context.Response.OutputStream.Close();
            }
            catch { }
        }
    }

    public void Dispose()
    {
        _listener?.Stop();
        _listener?.Close();
    }
}
