using Microsoft.JSInterop;

namespace Rubriki.Website.CookieCqrs;

public class CookieCommand(IJSRuntime jsRuntime, CookieCommand.Options options)
{
    public class Options
    {
        public int ExpiresDays { get; set; } = 300;
    }

    public async Task SetValue(string key, string value, int? days = null)
    {
        var curExp = days != null ? days > 0 ? DateToUTC(days.Value) : "" : DateToUTC(options.ExpiresDays);
        await SetCookie($"{key}={value}; expires={curExp}; path=/");
    }

    private async Task SetCookie(string value)
    {
        await jsRuntime.InvokeVoidAsync("eval", $"document.cookie = \"{value}\"");
    }

    private static string DateToUTC(int days) => DateTime.Now.AddDays(days).ToUniversalTime().ToString("R");
}
