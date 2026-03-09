import { InsightList } from './InsightList'
import { ScoreCard } from './ScoreCard'
import type { ResumeAnalysisResult } from '../types/resume'

interface AnalysisResultsProps {
  result: ResumeAnalysisResult | null
  isLoading: boolean
}

export function AnalysisResults({ result, isLoading }: AnalysisResultsProps) {
  if (isLoading) {
    return <p className="subtle">Generating analysis...</p>
  }

  if (!result) {
    return <p className="subtle">Submit a resume to view analysis results.</p>
  }

  return (
    <div className="results-grid">
      <div className="scores">
        <ScoreCard title="Overall Score" score={result.overallScore} />
        <ScoreCard title="ATS Score" score={result.atsScore} />
      </div>

      <article className="insight-group">
        <h3>Candidate Summary</h3>
        <p>{result.candidateSummary}</p>
      </article>

      <InsightList title="Strengths" items={result.strengths} />
      <InsightList title="Weaknesses" items={result.weaknesses} />
      <InsightList title="Missing Keywords" items={result.missingKeywords} />
      <InsightList title="Recommendations" items={result.recommendations} />
    </div>
  )
}
