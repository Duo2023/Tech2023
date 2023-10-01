using System.ComponentModel.DataAnnotations;

namespace Tech2023.DAL.Models;

/// <summary>
/// Represents a Cambridge resource, this is used to represent Cambridge exam papers
/// </summary>
public sealed class CambridgeResource : CustomResource
{
    /// <summary>
    /// What number the paper number
    /// </summary>
    [Required]
    public int Number { get; set; }

    /// <summary>
    /// This represents what season the <see cref="CambridgeResource"/> belongs to
    /// </summary>
    [Required]
    public Season Season { get; set; }

    /// <summary>
    /// What 'variant' of the paper the resource is
    /// </summary>
    [Required]
    public Variant Variant { get; set; }
}
