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
}
