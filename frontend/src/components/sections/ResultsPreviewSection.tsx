import type { ResumeAnalysisResponse } from '../../types/resumeAnalysis'
import { AnalysisResults } from '../AnalysisResults'
import { Section } from '../ui/Section'

interface ResultsPreviewSectionProps {
  result: ResumeAnalysisResponse | null
  isLoading: boolean
  errorMessage: string | null
  previewResult: ResumeAnalysisResponse
  hasRequestedAnalysis: boolean
}

export function ResultsPreviewSection({
  result,
  isLoading,
  errorMessage,
  previewResult,
  hasRequestedAnalysis,
}: ResultsPreviewSectionProps) {
  return (
    <Section id="analysis-preview" className="py-20">
      <div className="mb-8 max-w-2xl">
        <h2 className="text-3xl font-semibold text-white sm:text-4xl">See your analysis in a structured view</h2>
        <p className="mt-3 text-slate-300">
          Scores, summary, missing keywords, and practical recommendations all in one clear report.
        </p>
      </div>

      <AnalysisResults
        result={result}
        isLoading={isLoading}
        errorMessage={errorMessage}
        fallbackPreview={previewResult}
        hasRequestedAnalysis={hasRequestedAnalysis}
      />
    </Section>
  )
}
