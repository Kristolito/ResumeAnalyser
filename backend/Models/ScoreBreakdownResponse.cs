namespace ResumeAnalyser.Api.Models;

public sealed class ScoreBreakdownResponse
{
    public int Structure { get; set; }

    public int KeywordAlignment { get; set; }

    public int SkillsCoverage { get; set; }

    public int AchievementEvidence { get; set; }

    public int Readability { get; set; }
}
