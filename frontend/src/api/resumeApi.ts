import type { ResumeAnalysisPayload, ResumeAnalysisResult } from '../types/resume'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5112'

export const analyseResume = async (
  payload: ResumeAnalysisPayload,
): Promise<ResumeAnalysisResult> => {
  const formData = new FormData()
  formData.append('file', payload.file)
  formData.append('targetJobTitle', payload.targetJobTitle)
  formData.append('targetJobDescription', payload.targetJobDescription)

  if (payload.notes?.trim()) {
    formData.append('notes', payload.notes.trim())
  }

  const response = await fetch(`${API_BASE_URL}/api/resume/analyse`, {
    method: 'POST',
    body: formData,
  })

  if (!response.ok) {
    const message = await response.text()
    throw new Error(message || 'Failed to analyse resume.')
  }

  return (await response.json()) as ResumeAnalysisResult
}
