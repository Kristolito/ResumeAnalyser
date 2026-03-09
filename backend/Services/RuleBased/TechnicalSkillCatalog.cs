namespace ResumeAnalyser.Api.Services.RuleBased;

public static class TechnicalSkillCatalog
{
    public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Categories =
        new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["languages"] =
            [
                "c#", ".net", "java", "python", "javascript", "typescript", "go", "rust", "php", "kotlin", "swift", "sql"
            ],
            ["frameworks"] =
            [
                "asp.net", "react", "angular", "vue", "spring", "spring boot", "django", "flask", "node.js", "express", "next.js"
            ],
            ["cloud"] =
            [
                "aws", "azure", "gcp", "cloud", "lambda", "ec2", "s3", "aks", "eks", "kubernetes", "docker"
            ],
            ["databases"] =
            [
                "mysql", "postgresql", "sql server", "mongodb", "redis", "elasticsearch", "oracle", "dynamodb"
            ],
            ["devops"] =
            [
                "ci/cd", "jenkins", "github actions", "gitlab ci", "terraform", "ansible", "helm", "prometheus", "grafana"
            ],
            ["tooling"] =
            [
                "git", "jira", "postman", "swagger", "rest api", "microservices", "linux", "agile", "scrum"
            ]
        };

    public static IReadOnlyCollection<string> AllSkills =>
        Categories.SelectMany(pair => pair.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
}
