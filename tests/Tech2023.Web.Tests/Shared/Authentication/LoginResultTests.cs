using Tech2023.Web.Shared.Authentication;
using Xunit;

namespace Tech2023.Web.Tests.Shared.Authentication;

public class LoginResultTests
{
    [Theory]
    [ClassData(typeof(ErrorStringGenerator))]
    public void Success_ReturnsValidObject(string[] token)
    {
        var result = LoginResult.Ok(token[0]);

        Assert.NotNull(result);
        Assert.Equal(token[0], result.Token);
        Assert.True(result.Success);
        Assert.NotNull(result.Errors);
        Assert.False(result.Errors.Any());
    }

    [Theory]
    [ClassData(typeof(ErrorStringGenerator))]
    public void Fail_IsProperObject(string[] errors)
    {
        var result = LoginResult.Fail(errors);

        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Null(result.Token);
        Assert.Equal(errors, result.Errors);
    }
}
