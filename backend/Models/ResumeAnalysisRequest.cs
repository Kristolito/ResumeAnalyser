using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ResumeAnalyser.Api.Models;

public sealed class ResumeAnalysisRequest
{
    [FromForm(Name = "file")]
    [Required]
    public IFormFile? File { get; set; }

    [FromForm(Name = "targetJobTitle")]
    [Required]
    [MaxLength(150)]
    public string TargetJobTitle { get; set; } = string.Empty;

    [FromForm(Name = "targetJobDescription")]
    [Required]
    [MaxLength(10000)]
    public string TargetJobDescription { get; set; } = string.Empty;

    [FromForm(Name = "notes")]
    [MaxLength(5000)]
    public string? Notes { get; set; }
}
