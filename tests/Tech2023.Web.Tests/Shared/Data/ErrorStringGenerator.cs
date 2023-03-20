using System.Collections;

namespace Tech2023.Web.Tests.Shared.Authentication;

/// <summary>
/// Returns an IEnumerable{object[]} where each element is an array of string[]
/// </summary>
public class ErrorStringGenerator : IEnumerable<object[]>
{
    /// <summary>
    /// Returns the Enumerator of object[]
    /// </summary>
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new string[] { "error1", "error2" } };
        yield return new object[] { new string[] { "errrorarafa", "efafdsfasdf", "fejwafiaoefjaesiofjaseiof" } };
    }

    /// <summary>
    /// Stub method
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
