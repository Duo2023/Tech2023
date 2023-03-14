using Tech2023.Web.Shared.Authentication;
using Xunit;

namespace Tech2023.Web.Tests.Shared.Authentication;

public class AuthResultTests
{
    [Fact]
    public void Ok_IsValidResult()
    {
        var result = AuthResult.Ok();

        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Fail_ReturnsProperResults()
    {
        string[] errors = new string[]
        {
            "An error occured",
            "Another error occured"
        };

        var result = AuthResult.Fail(errors);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(errors, result.Errors);
    }
}
