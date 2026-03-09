import { AnalysisDetailSection } from './AnalysisDetailSection'
import { KeywordChips } from './KeywordChips'
import { RecommendationList } from './RecommendationList'
import { ScoreBreakdown } from './ScoreBreakdown'
import { ScoreCard } from '../ScoreCard'
import type { ResumeAnalysisResponse } from '../../types/resumeAnalysis'

interface AnalysisReportViewProps {
  title: string
  subtitle?: string
  result: ResumeAnalysisResponse
}

function SimpleList({ items }: { items: string[] }) {
  if (items.length === 0) {
    return <p className="text-sm text-slate-400">No items returned.</p>
  }

  return (
    <ul className="mt-3 space-y-2">
      {items.map((item) => (
        <li key={item} className="rounded-lg border border-white/10 bg-slate-950/60 px-3 py-2 text-sm text-slate-200">
          {item}
        </li>
      ))}
    </ul>
  )
}

export function AnalysisReportView({ title, subtitle, result }: AnalysisReportViewProps) {
  return (
    <article className="rounded-3xl border border-white/10 bg-slate-900/70 p-6">
      <header className="mb-5">
        <h3 className="text-2xl font-semibold text-white">{title}</h3>
        {subtitle ? <p className="mt-1 text-sm text-slate-400">{subtitle}</p> : null}
      </header>

      <div className="grid gap-4 lg:grid-cols-3">
        <div className="lg:col-span-2 grid gap-4 md:grid-cols-2">
          <ScoreCard title="Overall Score" score={result.overallScore} />
          <ScoreCard title="ATS Score" score={result.atsScore} />
        </div>
        <ScoreBreakdown breakdown={result.scoreBreakdown} />
      </div>

      <div className="mt-4">
        <AnalysisDetailSection title="Candidate Summary">
          <p className="text-sm leading-6 text-slate-300">{result.candidateSummary}</p>
        </AnalysisDetailSection>
      </div>

      <div className="mt-4 grid gap-4 lg:grid-cols-2">
        <AnalysisDetailSection title="Strengths">
          <SimpleList items={result.strengths} />
        </AnalysisDetailSection>
        <AnalysisDetailSection title="Improvement Areas">
          <SimpleList items={result.weaknesses} />
        </AnalysisDetailSection>
        <AnalysisDetailSection title="Missing Keywords">
          <KeywordChips keywords={result.missingKeywords} />
        </AnalysisDetailSection>
        <AnalysisDetailSection title="Next Actions">
          <RecommendationList recommendations={result.recommendations} />
        </AnalysisDetailSection>
      </div>
    </article>
  )
}
