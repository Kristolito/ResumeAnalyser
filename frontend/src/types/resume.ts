export interface ResumeAnalysisPayload {
  file: File
  targetJobTitle: string
  targetJobDescription: string
  notes?: string
}

export interface ResumeAnalysisResult {
  overallScore: number
  atsScore: number
  candidateSummary: string
  strengths: string[]
  weaknesses: string[]
  missingKeywords: string[]
  recommendations: string[]
}
