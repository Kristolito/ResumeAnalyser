import type { ResumeAnalysisResponse } from '../types/resumeAnalysis'
import { AnalysisReportView } from './analysis/AnalysisReportView'
import { Badge } from './ui/Badge'

interface AnalysisResultsProps {
  result: ResumeAnalysisResponse | null
  isLoading: boolean
  errorMessage?: string | null
  fallbackPreview?: ResumeAnalysisResponse
  hasRequestedAnalysis?: boolean
}

export function AnalysisResults({
  result,
  isLoading,
  errorMessage,
  fallbackPreview,
  hasRequestedAnalysis = false,
}: AnalysisResultsProps) {
  if (isLoading) {
    return (
      <div className="rounded-3xl border border-white/10 bg-slate-900/70 p-6">
        <p className="text-sm text-slate-300">Generating analysis...</p>
        <div className="mt-4 grid gap-3 md:grid-cols-2">
          <div className="h-24 animate-pulse rounded-2xl bg-slate-800/80" />
          <div className="h-24 animate-pulse rounded-2xl bg-slate-800/80" />
        </div>
        <div className="mt-3 h-28 animate-pulse rounded-2xl bg-slate-800/80" />
      </div>
    )
  }

  if (errorMessage) {
    return (
      <div className="rounded-2xl border border-red-400/40 bg-red-500/10 p-4 text-sm text-red-100" role="alert">
        {errorMessage}
      </div>
    )
  }

  const activeResult = result ?? fallbackPreview
  if (!activeResult) {
    return (
      <div className="rounded-3xl border border-white/10 bg-slate-900/70 p-6 text-sm text-slate-300">
        Submit your resume to see a complete analysis dashboard.
      </div>
    )
  }

  return (
    <div className="space-y-3">
      <div className="flex justify-end">
        {result ? (
          <Badge>Live Result</Badge>
        ) : hasRequestedAnalysis ? (
          <Badge>No Result</Badge>
        ) : (
          <Badge>Idle Preview</Badge>
        )}
      </div>
      <AnalysisReportView
        title="Detailed Analysis"
        subtitle="Structured report generated from your resume and role context."
        result={activeResult}
      />
    </div>
  )
}
