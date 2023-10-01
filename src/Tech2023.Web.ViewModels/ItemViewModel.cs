using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.ViewModels;

#nullable disable

public class ItemViewModel<TResource> where TResource : CustomResource
{
    public SubjectViewModel Subject { get; set; }

    public TResource Resource { get; set; }

    public Item Item { get; set; }
}

public class NceaItemViewModel : ItemViewModel<NceaResource> { }

public class CambridgeItemViewModel : ItemViewModel<CambridgeResource> { }
