using Tech2023.Web.Shared;
using System.Collections;

namespace Tech2023.Web.IntegrationTests.Sources;

public class AdminRouteSource<TSource> : RouteRetriever, IEnumerable<object[]>
{
    public AdminRouteSource() : base(typeof(TSource), Visiblity.Adminstrator)
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
