import { apiRequest } from './apiClient'
import type { ResumeAnalysisRequest, ResumeAnalysisResponse } from '../types/resumeAnalysis'

export async function analyseResume(
  request: ResumeAnalysisRequest,
): Promise<ResumeAnalysisResponse> {
  const formData = new FormData()
  formData.append('file', request.file)
  formData.append('targetJobTitle', request.targetJobTitle)
  formData.append('targetJobDescription', request.targetJobDescription)

  if (request.notes?.trim()) {
    formData.append('notes', request.notes.trim())
  }

  return apiRequest<ResumeAnalysisResponse>('/api/resume/analyse', {
    method: 'POST',
    body: formData,
  })
}
