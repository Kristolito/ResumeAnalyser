import { apiRequest } from './apiClient'
import type { ResumeAnalysisHistoryDetail, ResumeAnalysisHistoryItem } from '../types/history'

export async function getAnalysisHistory(): Promise<ResumeAnalysisHistoryItem[]> {
  return apiRequest<ResumeAnalysisHistoryItem[]>('/api/resume/analyses')
}

export async function getAnalysisHistoryById(id: string): Promise<ResumeAnalysisHistoryDetail> {
  return apiRequest<ResumeAnalysisHistoryDetail>(`/api/resume/analyses/${id}`)
}
