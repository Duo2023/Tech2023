using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.ViewModels;

#nullable disable

/// <summary>
/// Used for displaying an NCEA assessment item
/// </summary>
public class NceaItemViewModel : ItemViewModel<NceaResource> { }

/// <summary>
/// Used for displaying a Cambridge assessment item
/// </summary>
public class CambridgeItemViewModel : ItemViewModel<CambridgeResource> { }

/// <summary>
/// Used for displaying an assessment item
/// </summary>
/// <typeparam name="TResource">The assessment resource to use</typeparam>
public abstract class ItemViewModel<TResource> where TResource : CustomResource
{
    /// <summary>
    /// The subject that the <see cref="Item"/> belongs to
    /// </summary>
    public SubjectViewModel Subject { get; set; }

    /// <summary>
    /// The resource that the <see cref="Item"/> belongs to
    /// </summary>
    public TResource Resource { get; set; }

    /// <summary>
    /// The item to display
    /// </summary>
    public Item Item { get; set; }
}
