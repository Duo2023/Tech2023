using System.Collections;

using Tech2023.Web.Shared;

namespace Tech2023.Web.IntegrationTests.Sources;

public class PublicRouteSource<TRouteClass> : RouteRetriever, IEnumerable<object[]>
{
    public PublicRouteSource() : base(typeof(TRouteClass), Visiblity.Public)
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
