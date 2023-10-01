namespace Tech2023.Web.Caching;

/// <summary>
/// Cache slots for in memory storage
/// </summary>
public enum CacheSlots : byte
{
    /// <summary>Slot used for storing <see cref="DAL.Models.PrivacyPolicy"/></summary>
    PrivacyPolicy = 1,

    /// <summary>Slot used for storing <see cref="ViewModels.SubjectViewModel"/></summary>
    Subjects = 2
}
