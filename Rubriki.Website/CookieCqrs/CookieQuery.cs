using Microsoft.JSInterop;

namespace Rubriki.Website.CookieCqrs;

public class CookieQuery(IJSRuntime jsRuntime)
{
    public async Task<string> GetValue(string key, string def = "")
    {
        var cValue = await GetCookie();
        if (string.IsNullOrEmpty(cValue)) return def;

        var vals = cValue.Split(';');
        foreach (var val in vals)
            if (!string.IsNullOrEmpty(val) && val.IndexOf('=') > 0)
                if (val.Substring(0, val.IndexOf('=')).Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
                    return val.Substring(val.IndexOf('=') + 1);
        return def;
    }

    private async Task<string> GetCookie()
    {
        return await jsRuntime.InvokeAsync<string>("eval", $"document.cookie");
    }
}
