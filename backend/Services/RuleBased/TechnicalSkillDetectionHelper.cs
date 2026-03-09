using System.Text.RegularExpressions;

namespace ResumeAnalyser.Api.Services.RuleBased;

public static class TechnicalSkillDetectionHelper
{
    public static (int SkillHits, int CategoryCoverage, List<string> MatchedSkills) Detect(string normalizedResumeText)
    {
        var matchedSkills = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var matchedCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var category in TechnicalSkillCatalog.Categories)
        {
            var hasCategoryHit = false;
            foreach (var skill in category.Value)
            {
                if (!ContainsPhrase(normalizedResumeText, skill))
                {
                    continue;
                }

                matchedSkills.Add(skill);
                hasCategoryHit = true;
            }

            if (hasCategoryHit)
            {
                matchedCategories.Add(category.Key);
            }
        }

        return (matchedSkills.Count, matchedCategories.Count, matchedSkills.OrderBy(skill => skill).ToList());
    }

    private static bool ContainsPhrase(string text, string phrase)
    {
        var target = phrase.ToLowerInvariant();
        var searchStart = 0;

        while (searchStart < text.Length)
        {
            var index = text.IndexOf(target, searchStart, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return false;
            }

            var leftOk = index == 0 || !char.IsLetterOrDigit(text[index - 1]);
            var rightIndex = index + target.Length;
            var rightOk = rightIndex >= text.Length || !char.IsLetterOrDigit(text[rightIndex]);

            if (leftOk && rightOk)
            {
                return true;
            }

            searchStart = index + 1;
        }

        return false;
    }
}
