using System.Text;

using Microsoft.AspNetCore.WebUtilities;

using Tech2023.Web.Shared;

namespace Tech2023.Web.Tests.Shared;

public class EncoderTests
{
    [Theory]
    [InlineData("CfDJ8MctbNpBxHFBmO4TqGmL8VWIN78T7spH6IepX0JcuxCJyNXrMr4Dj1h6hrBdMUD+2DEIqz/n/7fXZBmAKuuFURfTUPvBbT3hSi8QXvUmbhmXteHHNM5RUUyGS4wMlWtg4s70h8mNz8Z5FkVPosFpDQ+fOtBWZov0g13MRE75mUAyKR7RzjZ9WkvEmp4/NDSawC3iK2kDx8gM2p5HB8lqnFMp2XRZXLLpQ4uO93Ob8f8dCcviR/YQUXToAqbhFPervA==")]
    public void Encode_ShouldHaveIdenticialBehaviour(string input)
    {
        string expected = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(input));

        if (WebEncoderHelpers.TryEncodeToUtf8Base64Url(input, out var result))
        {
            Assert.Equal(expected, result);
        }
        else
        {
            Assert.Fail("Encode method returned false");
        }
    }

    [Theory]
    [InlineData(null)]
    public void Encode_ShouldReturnFalse(string input)
    {
        bool result = WebEncoderHelpers.TryEncodeToUtf8Base64Url(input, out _);

        Assert.False(result);
    }

    [Theory]
    [InlineData("Q2ZESjhNY3RiTnBCeEhGQm1PNFRxR21MOFZXVWxwNW5TVCt5ejFONURGUVI4dGNyNG9sR2VxdWx5R054TThYV3lxdS9XclBMeWhCUzcwRGhtZ09UanNCNzA4eTZKOHBodEZydURaeU5DMlp4cU1NZVM0R0hOVnEwUXBaVytqSDNqN3AzMHBYdldTNnZLcHpJeXAvd1M2N2dvNjNtb1FSTDg5U0lmUEg0cVBjZkdNMGFKcXlYNlp0Z2Y5UVdLSjdmSU9KMXZ2R1VGS3hCUGNjblk4bU11WFljOVhIYWp6SHQ2cElleTdTMmwwTnZnb1JJSDliR3NoYno3ak9qdnY4NGQ5WWN0dz09")]
    public void Decode_ShouldHaveIdenticalBehaviour(string input)
    {
        string expected = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input));

        if (WebEncoderHelpers.TryDecodeFromBase64UrlEncoded(input, out var result))
        {
            Assert.Equal(expected, result);
        }
        else
        {
            Assert.Fail("Decode method returned false");
        }
    }
}
