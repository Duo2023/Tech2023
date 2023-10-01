namespace Tech2023.DAL;

/// <summary>
/// The type of ncea assesment that the ncea resource belongs to
/// </summary>
public enum NceaAssessmentType : byte
{
    /// <summary>
    /// The NCEA assement is an internal standard.
    /// </summary>
    Internal = 1,

    /// <summary>
    /// The NCEA assement is a an externally assessed standard, or like that of an exam that takes place at the end of the year
    /// </summary>
    External = 2,

    /// <summary>
    /// The NCEA assessment type is a special unit standard, these are very uncommon to occur and there are very few for them, and there might not be any examples or papers included on them
    /// </summary>
    Unit = 3
}
