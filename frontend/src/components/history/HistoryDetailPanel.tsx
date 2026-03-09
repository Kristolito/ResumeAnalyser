import { AnalysisReportView } from '../analysis/AnalysisReportView'
import type { ResumeAnalysisHistoryDetail } from '../../types/history'
import type { ResumeAnalysisResponse } from '../../types/resumeAnalysis'

interface HistoryDetailPanelProps {
  item: ResumeAnalysisHistoryDetail | null
  isLoading: boolean
  error: string | null
}

export function HistoryDetailPanel({ item, isLoading, error }: HistoryDetailPanelProps) {
  if (isLoading) {
    return <div className="rounded-2xl border border-white/10 bg-slate-900/60 p-6 text-slate-300">Loading analysis details...</div>
  }

  if (error) {
    return (
      <div className="rounded-2xl border border-red-400/40 bg-red-500/10 p-6 text-sm text-red-100">
        {error}
      </div>
    )
  }

  if (!item) {
    return (
      <div className="rounded-2xl border border-white/10 bg-slate-900/60 p-6 text-slate-300">
        Select an analysis to view full details.
      </div>
    )
  }

  const reportData: ResumeAnalysisResponse = {
    overallScore: item.overallScore,
    atsScore: item.atsScore,
    scoreBreakdown: item.scoreBreakdown,
    candidateSummary: item.candidateSummary,
    strengths: item.strengths,
    weaknesses: item.weaknesses,
    missingKeywords: item.missingKeywords,
    recommendations: item.recommendations,
  }

  return (
    <div className="space-y-4">
      <AnalysisReportView
        title={item.targetJobTitle}
        subtitle={`${item.originalFileName} · ${new Date(item.createdAt).toLocaleString()}`}
        result={reportData}
      />

      <div className="rounded-xl border border-white/10 bg-slate-950/60 p-4 text-sm text-slate-300">
        <p>
          <span className="font-semibold text-white">Target Job Description:</span> {item.targetJobDescription}
        </p>
        {item.notes ? (
          <p className="mt-2">
            <span className="font-semibold text-white">Notes:</span> {item.notes}
          </p>
        ) : null}
      </div>
    </div>
  )
}
