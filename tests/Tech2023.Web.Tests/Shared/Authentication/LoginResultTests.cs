using Tech2023.Web.Shared.Authentication;
using Xunit;

namespace Tech2023.Web.Tests.Shared.Authentication;

public class LoginResultTests
{
    [Theory]
    [InlineData("jasdofijasofpijaesfoi8easfjaseofijesaifojaseopfijsaofpijdsfopiajfepoiwfjaespoifjasopifjaseofpijszfoiesjf")]
    public void Success_ReturnsValidObject(string token)
    {
        var result = LoginResult.Ok(token);

        Assert.NotNull(result);
        Assert.Equal(token, result.Token);
        Assert.True(result.Success);
        Assert.False(result.Errors.Any());
    }
}
