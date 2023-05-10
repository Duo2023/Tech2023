using Microsoft.AspNetCore.Mvc;
using Tech2023.DAL.Models;

namespace Tech2023.Web.ViewComponents;

[ViewComponent]
public class SidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        List<Subject> subjects = new(4) // temporary stand in
        {
            new Subject()
            {
                Name = "Maths",
                DisplayColor = 0xef4444,
                Source = CurriculumSource.Cambridge
            },

            new Subject()
            {
                Name = "Physics",
                DisplayColor = 0x316aff,
                Source = CurriculumSource.Cambridge,
            },

            new Subject()
            {
                Name = "Chemistry",
                DisplayColor = 0xff7b16,
                Source = CurriculumSource.Ncea
            },

            new Subject()
            {
                Name = "IT",
                DisplayColor = 0xffc515,
                Source = CurriculumSource.Cambridge
            }
        };

        return View(subjects);
    }
}
