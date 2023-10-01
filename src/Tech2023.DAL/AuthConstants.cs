namespace Tech2023.DAL;

/// <summary>
/// Authentication constants used for validation
/// </summary>
public static class AuthConstants
{
    /// <summary>
    /// Requires that a password has the min length specified here
    /// </summary>
    /// <remarks>
    /// The number of possible passwords for this length can be calculated by [allowed-characted-count]^<see cref="MinPasswordLength"/> at worst case.
    /// eg. a numeric password with 4 digits allowed would be 10^4
    /// </remarks>
    public const int MinPasswordLength = 8;

    /// <summary>
    /// Error message for when the user has too short a password
    /// </summary>
    public const string PasswordLengthError = "Passwords must be 8+ characters long"; // update this value if you change MinPasswordLength
}
