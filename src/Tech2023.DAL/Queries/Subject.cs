using System.Diagnostics;

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

            if (await context.Subjects.AnyAsync(s => s.Name == subject.Name && s.Source == subject.Source))
            {
                Debug.WriteLine("Application tried creating a subject which already exists in all source");
                return;
            }

            context.Subjects.Add(subject);

            await context.SaveChangesAsync();
        }

        public static async Task<Subject?> FindSubjectAsync(ApplicationDbContext context, CurriculumSource source, string name)
        {
            Debug.Assert(context != null);

            name = name.ToUpper();

            return await context.Subjects
                .Where(s => s.Name == name)
                .Where(s => s.Source == source)
                .FirstOrDefaultAsync();
        }
    }
}
