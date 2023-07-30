using Tech2023.Web.IntegrationTests.Sources;
using Tech2023.Web.Shared;
using System.Collections;

namespace Tech2023.Web.IntegrationTests;

public class AuthenticatedRouteSource<TSource> : RouteRetriever, IEnumerable<object[]>
{
    public AuthenticatedRouteSource() : base(typeof(TSource), Visiblity.Authenticated)
    {
    }

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (var str in _routes)
        {
            yield return new object[] { str };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
