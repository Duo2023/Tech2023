using System.Diagnostics;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

public static partial class Queries
{
    public static class Subjects
    {
        public static async Task CreateSubjectAsync(ApplicationDbContext context, Subject subject)
        {
            ArgumentNullException.ThrowIfNull(subject);

            if (await context.Subjects.AnyAsync(s => s.Name == subject.Name && s.Source == subject.Source && s.Level == subject.Level))
            {
                Debug.WriteLine("Application tried creating a subject which already exists in all source");
                return;
            }

            context.Subjects.Add(subject);

            await context.SaveChangesAsync();
        }

        public static async Task<Subject?> FindSubjectAsync(ApplicationDbContext context, CurriculumSource source, CurriculumLevel level, string name)
        {
            Debug.Assert(context != null);

            name = name.ToUpper();

            return source switch
            {
                CurriculumSource.Ncea => await context.Subjects
                                        .Where(s => s.Name == name)
                                        .Where(s => s.Source == source)
                                        .Where(s => s.Level == level)
                                        .Include(s => s.NceaResource)
                                        .FirstOrDefaultAsync(),
                CurriculumSource.Cambridge => await context.Subjects
                                        .Where(s => s.Name == s.Name)
                                        .Where(s => s.Source == source)
                                        .Where(s => s.Level == level)
                                        .Include(s => s.CambridgeResource)
                                        .FirstOrDefaultAsync(),
                _ => await Task.FromResult<Subject?>(null),
            };
        }
    }
}
