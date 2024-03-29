﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tech2023.Web.ViewModels;

#nullable disable

/// <summary>
/// Subject list model, which contains a view only portion of the subjects and user subjects
/// </summary>
public class SubjectListModel
{
    /// <summary>
    /// The subjects that the user could join
    /// </summary>
    [Required]
    [JsonPropertyName("subjects")]
    public List<SubjectViewModel> AllSubjects { get; set; }

    /// <summary>
    /// The existing subjects that the user has
    /// </summary>
    [Required]
    public List<SubjectViewModel> SavedSubjects { get; set; }
}
