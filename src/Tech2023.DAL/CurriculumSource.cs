using System.ComponentModel.DataAnnotations;

namespace Tech2023.DAL;

/// <summary>
/// The type of curriculum a resource or model is attributed
/// </summary>
/// <remarks>
/// This is used for display purposes and is also used on the backend on web scraping
/// </remarks>
public enum CurriculumSource : byte
{
    /// <summary>
    /// NCEA curriculum is the provider
    /// </summary>
    [Display(Name = "NCEA")]
    Ncea = 1,

    /// <summary>
    /// Cambridge is the education provider
    /// </summary>
    [Display(Name = nameof(Cambridge))] // keep same because it looks fine
    Cambridge = 2,



    /* Do not change the underlying value of enum as this is constant and a binary breaking change 
     * This might have an equivalant layout in TypeScript front end
     */

    /* Open for more curriculums */
}

// 0b0000_0001
// 0b0000_0101
