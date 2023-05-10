namespace Tech2023.Core;

public static class HtmlHexString
{
    /// <summary>
    /// Gets the color value as a hexadecimal string value
    /// </summary>
    public static unsafe string GetHtmlHexString(uint color) => string.Create(9, color, (span, value) =>
    {
        span[0] = '#';

        _ = value.TryFormat(span[1..], out _, "x8");
    });
}
