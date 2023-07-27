using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Tech2023.DAL;
using Tech2023.DAL.Models;
using Tech2023.Web.Extensions;

namespace Tech2023.Web.ViewComponents;

[ViewComponent]
public class SidebarViewComponent : ViewComponent
{
    internal readonly UserManager<ApplicationUser> _userManager;

    public SidebarViewComponent(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.FindByUserAsync(UserClaimsPrincipal);

        return View(user.SavedSubjects);
    }

    //public IViewComponentResult Invoke()
    //{
    //    //List<Subject> subjects = new(4) // temporary stand in
    //    //{
    //    //    new Subject()
    //    //    {
    //    //        Name = "Maths",
    //    //        DisplayColor = 0xef4444,
    //    //        Source = CurriculumSource.Cambridge
    //    //    },

    //    //    new Subject()
    //    //    {
    //    //        Name = "Physics",
    //    //        DisplayColor = 0x316aff,
    //    //        Source = CurriculumSource.Cambridge,
    //    //    },

    //    //    new Subject()
    //    //    {
    //    //        Name = "Chemistry",
    //    //        DisplayColor = 0xff7b16,
    //    //        Source = CurriculumSource.Ncea
    //    //    },

    //    //    new Subject()
    //    //    {
    //    //        Name = "IT",
    //    //        DisplayColor = 0xffc515,
    //    //        Source = CurriculumSource.Cambridge
    //    //    }
    //    //};

    //    return View(subjects);
    //}
}
