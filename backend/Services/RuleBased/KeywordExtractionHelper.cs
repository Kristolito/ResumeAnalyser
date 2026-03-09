using System.Text.RegularExpressions;

namespace ResumeAnalyser.Api.Services.RuleBased;

public static class KeywordExtractionHelper
{
    private static readonly Regex TokenRegex = new(@"\b[a-zA-Z][a-zA-Z0-9+#./-]{2,}\b", RegexOptions.Compiled);
    private static readonly HashSet<string> StopWords = new(
    [
        "the", "and", "for", "with", "from", "that", "this", "into", "across", "within", "about", "have", "has", "had",
        "are", "was", "were", "you", "your", "our", "their", "they", "them", "role", "job", "candidate", "required",
        "preferred", "experience", "strong", "ability", "skills", "skill", "plus", "using", "use", "used", "will",
        "should", "must", "can", "may", "such", "also", "more", "than", "all", "any", "not"
    ], StringComparer.OrdinalIgnoreCase);

    public static List<string> BuildKeywordTargets(string targetJobTitle, string targetJobDescription, string? notes)
    {
        var source = $"{targetJobTitle}\n{targetJobDescription}\n{notes}";
        var normalized = source.ToLowerInvariant();
        var weighted = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var skill in TechnicalSkillCatalog.AllSkills)
        {
            if (ContainsPhrase(normalized, skill))
            {
                AddWeight(weighted, skill, targetJobTitle.Contains(skill, StringComparison.OrdinalIgnoreCase) ? 6 : 4);
            }
        }

        var titleTokens = ExtractTokens(targetJobTitle);
        foreach (var token in titleTokens)
        {
            AddWeight(weighted, token, 5);
        }

        var descriptionTokens = ExtractTokens(targetJobDescription);
        foreach (var token in descriptionTokens)
        {
            AddWeight(weighted, token, 2);
        }

        var noteTokens = ExtractTokens(notes ?? string.Empty);
        foreach (var token in noteTokens)
        {
            AddWeight(weighted, token, 1);
        }

        return weighted
            .OrderByDescending(pair => pair.Value)
            .ThenBy(pair => pair.Key, StringComparer.OrdinalIgnoreCase)
            .Select(pair => pair.Key)
            .Take(26)
            .ToList();
    }

    public static List<string> FindMissingKeywords(string normalizedResumeText, IReadOnlyList<string> targets, int max = 10)
    {
        return targets
            .Where(target => !ContainsPhrase(normalizedResumeText, target))
            .Take(max)
            .ToList();
    }

    public static List<string> FindMatchedKeywords(string normalizedResumeText, IReadOnlyList<string> targets)
    {
        return targets.Where(target => ContainsPhrase(normalizedResumeText, target)).ToList();
    }

    private static IEnumerable<string> ExtractTokens(string text)
    {
        return TokenRegex.Matches(text.ToLowerInvariant())
            .Select(match => match.Value.Trim())
            .Where(token => token.Length >= 3)
            .Where(token => !StopWords.Contains(token));
    }

    private static void AddWeight(IDictionary<string, int> weighted, string token, int weight)
    {
        if (weighted.ContainsKey(token))
        {
            weighted[token] += weight;
            return;
        }

        weighted[token] = weight;
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
