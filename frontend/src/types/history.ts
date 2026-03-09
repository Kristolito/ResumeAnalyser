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
