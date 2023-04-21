
using Microsoft.AspNetCore.Mvc;
using Tech2023.Web.Models;

namespace Tech2023.Web.ViewComponents;

[ViewComponent]
public class SidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        List<Subject> subjects = new List<Subject>();
        subjects.Add(new Subject("#ef4444", CourseProvider.CAIE, "Maths", ""));
        subjects.Add(new Subject("#316aff", CourseProvider.CAIE, "Physics", ""));
        subjects.Add(new Subject("#ff7b16", CourseProvider.CAIE, "Chemistry", ""));
        subjects.Add(new Subject("#ffe800", CourseProvider.CAIE, "IT", ""));
        return View(subjects);
    }
}