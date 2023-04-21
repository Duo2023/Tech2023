
using Microsoft.AspNetCore.Mvc;

namespace Tech2023.Web.ViewComponents;

[ViewComponent]
public class SidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}