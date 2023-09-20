using System.Diagnostics;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

public static partial class Queries
{
    public static class Subjects
    {
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

        public static async Task AddSubjectsAsync(ApplicationDbContext context, ApplicationUser user, IEnumerable<Subject> subjects)
        {
            user.SavedSubjects.AddRange(subjects);

            user.SyncLatest();

            context.Users.Update(user);

            await context.SaveChangesAsync();
        }
    }
}
