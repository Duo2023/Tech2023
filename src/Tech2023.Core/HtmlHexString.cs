namespace Tech2023.Core;

public static class HtmlHexString
{
    /// <summary>
    /// Gets the color value as a hexadecimal string value
    /// </summary>
    public static unsafe string GetHtmlHexString(uint color)
    {
        return string.Create(9, color, (s, c) =>
        {
            ref byte source = ref Unsafe.As<uint, byte>(ref color);
            ref char destination = ref MemoryMarshal.GetReference(s);

        });
    }
}
