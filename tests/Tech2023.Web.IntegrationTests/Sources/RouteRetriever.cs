using System.Reflection;

using Tech2023.Web.Shared;

namespace Tech2023.Web.IntegrationTests.Sources;

// TODO: Rearrange the data sources and fix this somewhat of a mess

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
            foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.Static)) // it should be public
            {
                if (!(field.IsLiteral && field.FieldType == typeof(string))) // this field should be declared as const string
                {
                    continue;
                }

                var attribute = field.GetCustomAttribute<RouteVisibilityAttribute>(false); // it should be tagged with [RouteVisibility(Visibility.?)]

                if (attribute is null)
                {
                    continue;
                }

                if (attribute.Visiblity != visiblity)
                {
                    continue; // the visiblity should be matched
                }

                string route = (string)field.GetRawConstantValue()!; // extract the string route

                if (attribute.IsRelative)
                {
                    route = attribute.BaseRoute!.CombinePaths(route); // check whether its relative to another uri specified
                }

                _routes.Add(route); // add the route
            }
        }
    }
}
