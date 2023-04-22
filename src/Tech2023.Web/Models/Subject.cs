namespace Tech2023.Web.Models;

public class Subject
{
    public string Color { get; set; }
    public string Title { get; set; }
    public string PaperUrl { get; set; }
    public CourseProvider CourseProvider { get; set; }

    public Subject(string color, CourseProvider courseProvider, string title, string paperUrl)
    {
        Color = color;
        CourseProvider = courseProvider;
        Title = title;
        PaperUrl = paperUrl;
    }
}

public enum CourseProvider
{
    NCEA,
    CAIE
}