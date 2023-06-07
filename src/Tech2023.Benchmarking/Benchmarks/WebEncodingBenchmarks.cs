using System.Text;
using Tech2023.Web.Shared;
using Microsoft.AspNetCore.WebUtilities;

namespace Tech2023.Benchmarking;

[MemoryDiagnoser]
public class WebEncodingTests
{
    internal const string ToDecode = "Q2ZESjhNY3RiTnBCeEhGQm1PNFRxR21MOFZXVWxwNW5TVCt5ejFONURGUVI4dGNyNG9sR2VxdWx5R054TThYV3lxdS9XclBMeWhCUzcwRGhtZ09UanNCNzA4eTZKOHBodEZydURaeU5DMlp4cU1NZVM0R0hOVnEwUXBaVytqSDNqN3AzMHBYdldTNnZLcHpJeXAvd1M2N2dvNjNtb1FSTDg5U0lmUEg0cVBjZkdNMGFKcXlYNlp0Z2Y5UVdLSjdmSU9KMXZ2R1VGS3hCUGNjblk4bU11WFljOVhIYWp6SHQ2cElleTdTMmwwTnZnb1JJSDliR3NoYno3ak9qdnY4NGQ5WWN0dz09";
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

    [Benchmark]
    public string ImplementationDecode()
    {
        WebEncoderHelpers.TryDecodeFromBase64UrlEncoded(ToDecode, out var s);

        return s!;
    }

    [Benchmark]
    public string OriginalDecode()
    {
        return Impl.Decode(ToDecode);
    }
}

file class Impl
{
    public static string Decode(string input)
    {
        return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input));
    }
    public static string GetString(string input)
    {
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(input));
    }
}
