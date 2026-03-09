using System.Text.RegularExpressions;

namespace ResumeAnalyser.Api.Services.RuleBased;

public static class RuleBasedTextNormalizer
{
    private static readonly Regex MultiWhitespaceRegex = new(@"\s+", RegexOptions.Compiled);
    private static readonly Regex EmptyLineRegex = new(@"^\s*$", RegexOptions.Compiled);

    public static string NormalizeForMatching(string text)
    {
        return MultiWhitespaceRegex.Replace(text.ToLowerInvariant(), " ").Trim();
    }

    public static IReadOnlyList<string> SplitMeaningfulLines(string text)
    {
        return text
            .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !EmptyLineRegex.IsMatch(line))
            .ToList();
    }
}
