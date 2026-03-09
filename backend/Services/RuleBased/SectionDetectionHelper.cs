using System.Text.RegularExpressions;

namespace ResumeAnalyser.Api.Services.RuleBased;

public static class SectionDetectionHelper
{
    public static readonly IReadOnlyDictionary<string, string[]> SectionHeadingMap =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["summary"] = ["summary", "professional summary", "profile", "about me"],
            ["experience"] = ["experience", "work experience", "employment", "professional experience"],
            ["skills"] = ["skills", "technical skills", "core competencies", "technologies"],
            ["education"] = ["education", "academic background", "qualifications"],
            ["projects"] = ["projects", "project experience", "key projects"],
            ["certifications"] = ["certifications", "licenses", "certificates"]
        };

    public static (int SectionCount, List<string> MissingSections) DetectSections(IReadOnlyList<string> lines)
    {
        var lowerLines = lines.Select(line => line.ToLowerInvariant()).ToList();
        var found = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var section in SectionHeadingMap)
        {
            var exists = section.Value.Any(heading =>
                lowerLines.Any(line => Regex.IsMatch(line, $@"^\s*{Regex.Escape(heading)}\s*:?\s*$")));

            if (exists)
            {
                found.Add(section.Key);
            }
        }

        var missing = SectionHeadingMap.Keys
            .Where(key => !found.Contains(key))
            .ToList();

        return (found.Count, missing);
    }
}
