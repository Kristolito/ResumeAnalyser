export interface ResumeAnalysisRequest {
  file: File
  targetJobTitle: string
  targetJobDescription: string
  notes?: string
}

export interface ResumeAnalysisResponse {
  overallScore: number
  atsScore: number
  candidateSummary: string
  strengths: string[]
  weaknesses: string[]
  missingKeywords: string[]
  recommendations: string[]
}
