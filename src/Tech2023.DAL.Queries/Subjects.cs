using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tech2023.DAL.Identity;
using Tech2023.DAL.Models;
using Tech2023.Web.ViewModels;

namespace Tech2023.DAL.Queries;

public static class Subjects
{
    internal static Func<ApplicationDbContext, Subject, Task<bool>> _existsAsync =
        EF.CompileAsyncQuery((ApplicationDbContext context, Subject subject) => context.Subjects.Any(s => s.Name == subject.Name && s.Source == subject.Source && s.Level == subject.Level));

    /// <summary>
    /// Creates a subject in the specified DbContext if it doesn't exist already
    /// </summary>
    /// <param name="context"></param>
    /// <param name="subject"></param>
    /// <exception cref="ArgumentNullException">If context or subject is <see langword="null"/> </exception>
    /// <returns></returns>
    public static async Task CreateSubjectAsync(ApplicationDbContext context, Subject subject)
    {
        // parameters are validated in release and debug builds 
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(subject);

        subject.Name = subject.Name.ToUpper();

        if (await _existsAsync(context, subject))
        {
            Debug.WriteLine("Application tried creating a subject which already exists in all source");
            return;
        }

        context.Subjects.Add(subject);

        await context.SaveChangesAsync();
    }


    public static async Task CreateSubjectsAsync(ApplicationDbContext context, IEnumerable<Subject> subjects)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(subjects);

        foreach (var subject in subjects)
        {
            subject.Name = subject.Name.ToUpper();
        }

        subjects = subjects.Distinct();

        await context.Subjects.AddRangeAsync(subjects);

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Finds the subject in the database context using the name and the source and level of the subject
    /// </summary>
    /// <param name="context">The database context to operate on</param>
    /// <param name="source">The curriculum source</param>
    /// <param name="level">The curriculum level</param>
    /// <param name="name">The name of the subject</param>
    /// <returns>This method returns a <see langword="null"/> if <see cref="Subject"/> is</returns>
    public static async Task<Subject?> FindSubjectAsync(ApplicationDbContext context, CurriculumSource source, CurriculumLevel level, string name)
    {
        Debug.Assert(context != null);

        name = name.ToUpper();

        // match the query so that the source, level and name are matched first
        var query = context.Subjects
            .Where(s => s.Name == name)
            .Where(s => s.Source == source)
            .Where(s => s.Level == level);

        // build the query based on the level
        return source switch
        {
            CurriculumSource.Ncea => await query.Include(s => s.NceaResource).FirstOrDefaultAsync(),
            CurriculumSource.Cambridge => await query.Include(s => s.CambridgeResource).FirstOrDefaultAsync(),
            _ => null,
        };
    }

    /// <summary>
    /// Adds the specified <see cref="ApplicationUser"/> to the <see cref="Subject"/>'s provided
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="user">The database user</param>
    /// <param name="subjects"></param>
    /// <returns><see cref="Task"/> to be awaited on</returns>
    public static async Task AddSubjectsAsync(ApplicationDbContext context, ApplicationUser user, IEnumerable<Subject> subjects)
    {
        user.SavedSubjects.AddRange(subjects);

        user.SyncLatest();

        context.Users.Update(user);

        await context.SaveChangesAsync();
    }

    public static async Task<List<SubjectViewModel>> GetAllSubjectViewModelsAsync(ApplicationDbContext context) => await context.Subjects.Select(s => (SubjectViewModel)s).ToListAsync();
}
