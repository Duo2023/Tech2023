using System.ComponentModel.DataAnnotations;

namespace Tech2023.Web.Shared;

#nullable disable

public class Register
{
    /// <summary>
    /// The email address to the target associated account
    /// </summary>
    [EmailAddress]
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// The password for the target user account
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}
