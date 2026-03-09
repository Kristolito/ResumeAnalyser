import { env } from '../config/env'
import type { ResumeAnalysisHistoryDetail, ResumeAnalysisHistoryItem } from '../types/history'

async function parseErrorMessage(response: Response): Promise<string> {
  const message = await response.text()
  return message || 'Failed to load analysis history.'
}

export async function getAnalysisHistory(): Promise<ResumeAnalysisHistoryItem[]> {
  const response = await fetch(`${env.apiBaseUrl}/api/resume/analyses`)
  if (!response.ok) {
    throw new Error(await parseErrorMessage(response))
  }

  return (await response.json()) as ResumeAnalysisHistoryItem[]
}

export async function getAnalysisHistoryById(id: string): Promise<ResumeAnalysisHistoryDetail> {
  const response = await fetch(`${env.apiBaseUrl}/api/resume/analyses/${id}`)
  if (!response.ok) {
    throw new Error(await parseErrorMessage(response))
  }

  return (await response.json()) as ResumeAnalysisHistoryDetail
}
