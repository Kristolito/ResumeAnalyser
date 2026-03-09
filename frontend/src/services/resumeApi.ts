import { env } from '../config/env'
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

  const response = await fetch(`${env.apiBaseUrl}/api/resume/analyse`, {
    method: 'POST',
    body: formData,
  })

  if (!response.ok) {
    const errorMessage = await response.text()
    throw new Error(errorMessage || 'Could not analyse the resume. Please try again.')
  }

  return (await response.json()) as ResumeAnalysisResponse
}
