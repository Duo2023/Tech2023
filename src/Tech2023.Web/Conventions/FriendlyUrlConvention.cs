using System.Diagnostics;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Tech2023.Web.Conventions;

/// <summary>
/// Modifies razor pages to use friendly urls which make them readable and less usless
/// </summary>
public class FriendlyUrlConvention : IPageRouteModelConvention
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FriendlyUrlConvention"/> class
    /// </summary>
    /// <param name="areaName">The area that this applies to</param>
    public FriendlyUrlConvention(string areaName)
    {
        ArgumentNullException.ThrowIfNull(areaName);

        AreaName = areaName;
    }

    /// <summary>
    /// The area name that the friendly url convention removes from the path
    /// </summary>
    public string AreaName { get; }

    /// <inheritdoc/>
    public void Apply(PageRouteModel model)
    {
        Debug.Assert(model != null);

        if (model.RelativePath.StartsWith($"/Areas/{AreaName}"))
        {
            foreach (SelectorModel selector in model.Selectors)
            {
                if (selector.AttributeRouteModel is null)
                {
                    continue;
                }

                selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template?.Replace(AreaName, string.Empty);
            }
        }
    }
}
