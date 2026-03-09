using ResumeAnalyser.Api.Services.RuleBased;

namespace ResumeAnalyser.Api.Tests.RuleBased;

public sealed class KeywordExtractionHelperTests
{
    [Fact]
    public void BuildKeywordTargets_PrioritizesTechnicalTermsFromJobText()
    {
        const string title = "Senior Backend Engineer";
        const string description =
            "We need strong .NET, C#, Azure, Kubernetes, Docker, Microservices, and CI/CD experience.";

        var targets = KeywordExtractionHelper.BuildKeywordTargets(title, description, null);

        Assert.Contains(".net", targets);
        Assert.Contains("azure", targets);
        Assert.Contains("kubernetes", targets);
        Assert.Contains("microservices", targets);
    }

    [Fact]
    public void FindMissingKeywords_ReturnsUnmatchedTerms()
    {
        const string normalizedResume = "experienced backend engineer with c# and sql expertise";
        var targets = new List<string> { "c#", "azure", "kubernetes", "sql" };

        var missing = KeywordExtractionHelper.FindMissingKeywords(normalizedResume, targets);

        Assert.Contains("azure", missing);
        Assert.Contains("kubernetes", missing);
        Assert.DoesNotContain("c#", missing);
    }
}
