export interface ResumeAnalysisRequest {
  file: File
  targetJobTitle: string
  targetJobDescription: string
  notes?: string
}

export interface ResumeAnalysisResponse {
  overallScore: number
  atsScore: number
  scoreBreakdown?: {
    structure: number
    keywordAlignment: number
    skillsCoverage: number
    achievementEvidence: number
    readability: number
  }
  candidateSummary: string
  strengths: string[]
  weaknesses: string[]
  missingKeywords: string[]
  recommendations: string[]
}
