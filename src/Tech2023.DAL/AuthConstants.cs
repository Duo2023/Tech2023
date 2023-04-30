namespace Tech2023.DAL;

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

    public const string PasswordLengthError = "The password length must be 8 or greater"; // update this value if you change MinPasswordLength
}
