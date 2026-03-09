using System.Text.RegularExpressions;

namespace ResumeAnalyser.Api.Services.RuleBased;

public static class AchievementDetectionHelper
{
    private static readonly Regex PercentRegex = new(@"\b\d+([.,]\d+)?\s?%\b", RegexOptions.Compiled);
    private static readonly Regex CurrencyRegex = new(@"(\$|£|€)\s?\d+([.,]\d+)?([kKmMbB])?", RegexOptions.Compiled);
    private static readonly Regex NumericRegex = new(@"\b\d+([.,]\d+)?([kKmMbB])?\b", RegexOptions.Compiled);
    private static readonly Regex ScaleRegex = new(@"\b(million|billion|thousand|global|enterprise|large-scale|high-volume)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly string[] ActionVerbs =
    [
        "increased", "reduced", "improved", "delivered", "led", "built", "optimized", "launched", "implemented", "designed",
        "automated", "scaled", "managed", "owned", "drove", "accelerated", "transformed", "streamlined", "mentored", "deployed"
    ];

    private static readonly string[] ImpactPhrases =
    [
        "resulting in", "which led to", "driving", "impact", "outcome", "customer satisfaction", "cost savings",
        "performance improvement", "time reduction", "revenue growth"
    ];

    public static (int NumericCount, int PercentCount, int CurrencyCount, int ScaleCount, int ActionVerbCount, int ImpactPhraseCount)
        Analyze(string normalizedText)
    {
        var lower = normalizedText.ToLowerInvariant();
        var actionHits = ActionVerbs.Count(verb => ContainsWord(lower, verb));
        var impactHits = ImpactPhrases.Count(phrase => lower.Contains(phrase, StringComparison.OrdinalIgnoreCase));

        return (
            NumericRegex.Matches(normalizedText).Count,
            PercentRegex.Matches(normalizedText).Count,
            CurrencyRegex.Matches(normalizedText).Count,
            ScaleRegex.Matches(normalizedText).Count,
            actionHits,
            impactHits
        );
    }

    private static bool ContainsWord(string text, string term)
    {
        var escaped = Regex.Escape(term);
        return Regex.IsMatch(text, $@"\b{escaped}\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
}
