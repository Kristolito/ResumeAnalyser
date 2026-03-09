using ResumeAnalyser.Api.Services.RuleBased;

namespace ResumeAnalyser.Api.Tests.RuleBased;

public sealed class SectionDetectionHelperTests
{
    [Fact]
    public void DetectSections_FindsExpectedHeadings()
    {
        var lines = new List<string>
        {
            "Professional Summary",
            "Work Experience",
            "Technical Skills",
            "Education",
            "Projects"
        };

        var (count, missing) = SectionDetectionHelper.DetectSections(lines);

        Assert.True(count >= 5);
        Assert.DoesNotContain("summary", missing);
        Assert.DoesNotContain("experience", missing);
    }
}
