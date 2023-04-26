using System.Collections;

using Tech2023.Web.Shared;

namespace Tech2023.Web.IntegrationTests.Sources;

internal class PublicRouteSource : IEnumerable<object[]>
{
    internal readonly List<string> _routes = new();

    public PublicRouteSource() => GetAllPublicRoutes();

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (var str in _routes)
        {
            yield return new object[] { str };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    internal void GetAllPublicRoutes()
    {
        var routes = typeof(Routes);

        AddForClass(routes);

        foreach (var t in routes.GetNestedTypes())
        {
            AddForClass(t);
        }

        void AddForClass(Type classType)
        {
            foreach (var type in classType.GetFields())
            {
                if (type.CustomAttributes.Any(t => t.AttributeType == typeof(PublicRouteAttribute) && type.FieldType == typeof(string)))
                {
                    _routes.Add((string)type.GetValue(null)!);
                }
            }
        }
    }
}
