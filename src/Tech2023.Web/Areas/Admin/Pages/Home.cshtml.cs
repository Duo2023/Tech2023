using System.Diagnostics;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Tech2023.Web.Areas.Admin.Pages;

public class HomeModel : PageModel
{
    public void OnGet()
    {
        Debug.WriteLine("Administrator page accessed");
    }
}
