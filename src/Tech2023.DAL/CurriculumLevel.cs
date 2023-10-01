namespace Tech2023.DAL;

/*
 * If you change this type you will have to change the implementation of Curriculum.cs and CurriculumSource.cs
 * You will also have to handle the new case across the codebase
 */

/// <summary>
/// The level of curriculum in the belonging to a <see cref="CurriculumSource"/>
/// </summary>
public enum CurriculumLevel : byte
{
    /// <summary>Either NCEA L1 or Cambridge IGSCE</summary>
    L1 = 1,

    /// <summary>Either NCEA L2 or Cambridge AS/A</summary>
    L2 = 2,

    /// <summary>Either NCEA L3 or Cambridge A2</summary>
    L3 = 3
}
