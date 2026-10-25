using System.Text.RegularExpressions;

namespace StarWarsUnlimited.Core.Http;

/// <summary>
/// Simple HTTP request router.
/// </summary>
public class RequestRouter
{
    private readonly List<RouteEntry> _routes = new();

    public void Map(string pattern, Func<RequestContext, (int statusCode, string content)> handler)
    {
        _routes.Add(new RouteEntry(pattern, handler));
    }

    public (int statusCode, string content)? Dispatch(string method, string path, string? body)
    {
        foreach (var route in _routes)
        {
            var match = Regex.Match(path, "^" + PatternToRegex(route.Pattern));
            if (match.Success)
            {
                var ctx = new RequestContext(match, method, body);
                return route.Handler(ctx);
            }
        }
        return null;
    }

    private static string PatternToRegex(string pattern)
    {
        return pattern.Replace("{", "(?<").Replace("}", ">").Replace("\\d", "[0-9]").Replace(".", "\\.");
    }

    private record RouteEntry(string Pattern, Func<RequestContext, (int statusCode, string content)> Handler);

    /// <summary>
    /// Context for HTTP request routing.
    /// </summary>
    public class RequestContext
    {
        public readonly string Method;
        public readonly string? Body;
        public readonly Dictionary<string, string> RouteValues;

        public RequestContext(Match match, string method, string? body)
        {
            Method = method;
            Body = body;
            RouteValues = new Dictionary<string, string>();

            // Extract named groups from the match
            foreach (System.Text.RegularExpressions.Group group in match.Groups)
            {
                if (group.Success && group.Name != "0")
                {
                    RouteValues[group.Name] = group.Value;
                }
            }
        }

        public string this[string key] => RouteValues.TryGetValue(key, out var v) ? v : "";
    }
}
