export interface ResumeAnalysisHistoryItem {
  id: string
  originalFileName: string
  targetJobTitle: string
  overallScore: number
  atsScore: number
  createdAt: string
}

export interface ResumeAnalysisHistoryDetail {
  id: string
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
  targetJobTitle: string
  targetJobDescription: string
  notes?: string
  originalFileName: string
  createdAt: string
}
