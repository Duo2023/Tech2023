using System.Text;
using Tech2023.Web.Shared;
using Microsoft.AspNetCore.WebUtilities;

namespace Tech2023.Benchmarking;

[MemoryDiagnoser]
public class WebEncodingTests
{
    internal const string Input
        = "CfDJ8MctbNpBxHFBmO4TqGmL8VWIN78T7spH6IepX0JcuxCJyNXrMr4Dj1h6hrBdMUD+2DEIqz/n/7fXZBmAKuuFURfTUPvBbT3hSi8QXvUmbhmXteHHNM5RUUyGS4wMlWtg4s70h8mNz8Z5FkVPosFpDQ+fOtBWZov0g13MRE75mUAyKR7RzjZ9WkvEmp4/NDSawC3iK2kDx8gM2p5HB8lqnFMp2XRZXLLpQ4uO93Ob8f8dCcviR/YQUXToAqbhFPervA==";

    [Benchmark(Baseline = true)]
    public string Implementation()
    {
        WebEncoderHelpers.TryEncodeToUtf8Base64Url(Input, out var s);

        return s!;
    }

    [Benchmark]
    public string Original()
    {
        return Impl.GetString(Input);
    }
}

file class Impl
{
    public static string GetString(string input)
    {
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(input));
    }
}
