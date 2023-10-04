using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.Initialization.Generators;

// generates items for resources 

internal readonly struct ItemGenerator : IGenerator<Item>
{
    public Item Generate()
    {
        var item = new Item()
        {
            Source = "/assets/dev/dev_test_pdf.pdf",
            Year = Random.Shared.Next(2012, DateTime.Now.Year),
            Created = DateTimeOffset.UtcNow
        };

        item.SetToCurrent();

        return item;
    }
}
