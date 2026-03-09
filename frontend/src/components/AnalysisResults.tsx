import type { ResumeAnalysisResponse } from '../types/resumeAnalysis'
import { InsightList } from './InsightList'
import { ScoreCard } from './ScoreCard'
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
    <div className="rounded-3xl border border-white/10 bg-slate-900/70 p-6">
      <div className="mb-5 flex items-center justify-between">
        <h3 className="text-xl font-semibold text-white">Analysis Preview</h3>
        {result ? <Badge>Live Result</Badge> : hasRequestedAnalysis ? <Badge>No Result</Badge> : <Badge>Idle Preview</Badge>}
      </div>

      <div className="grid gap-4 md:grid-cols-2">
        <ScoreCard title="Overall Score" score={activeResult.overallScore} />
        <ScoreCard title="ResumeAnalyser Score" score={activeResult.atsScore} />
      </div>

      <article className="mt-4 rounded-2xl border border-white/10 bg-slate-900/60 p-5">
        <h4 className="text-base font-semibold text-white">Candidate Summary</h4>
        <p className="mt-2 text-sm leading-6 text-slate-300">{activeResult.candidateSummary}</p>
      </article>

      <div className="mt-4 grid gap-4 lg:grid-cols-2">
        <InsightList title="Strengths" items={activeResult.strengths} />
        <InsightList title="Weaknesses" items={activeResult.weaknesses} />
        <InsightList title="Missing Keywords" items={activeResult.missingKeywords} />
        <InsightList title="Recommendations" items={activeResult.recommendations} />
      </div>
    </div>
  )
}
