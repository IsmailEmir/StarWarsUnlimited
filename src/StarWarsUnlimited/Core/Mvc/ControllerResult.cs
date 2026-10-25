namespace StarWarsUnlimited.Core.Mvc;

/// <summary>
/// Результат действия контроллера.
/// </summary>
public class ControllerResult
{
    public int StatusCode { get; set; }
    public string? StatusDescription { get; set; }
    public string? ContentType { get; set; }
    public object? Content { get; set; }

    public ControllerResult(int statusCode, string? statusDescription = null, string? contentType = null, object? content = null)
    {
        StatusCode = statusCode;
        StatusDescription = statusDescription;
        ContentType = contentType;
        Content = content;
    }
}
