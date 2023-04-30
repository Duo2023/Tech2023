using System.Reflection;

using Tech2023.Web.Shared;

namespace Tech2023.Web.IntegrationTests.Sources;

public abstract class RouteRetriever
{
    internal readonly List<string> _routes = new();

    public RouteRetriever(Type type, Visiblity visiblity)
    {
        GetRoutes(type, visiblity);
    }

    internal void GetRoutes(Type t, Visiblity visiblity)
    {
        AddForType(t);

        foreach (var type in t.GetNestedTypes())
        {
            AddForType(type);
        }

        void AddForType(Type t)
        {
            foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (!(field.IsLiteral && field.FieldType == typeof(string)))
                {
                    continue;
                }

                var attribute = field.GetCustomAttribute<RouteVisibilityAttribute>(false);

                if (attribute is null)
                {
                    continue;
                }

                if (attribute.Visiblity != visiblity)
                {
                    continue;
                }

                string route = (string)field.GetRawConstantValue()!;

                if (attribute.IsRelative)
                {
                    route = attribute.BaseRoute!.CombinePaths(route);
                }

                _routes.Add(route);
            }
        }
    }
}
