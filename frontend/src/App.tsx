import { useMutation } from '@tanstack/react-query'
import { useState } from 'react'
import { analyseResume } from './api/resumeApi'
import { FeaturesSection } from './components/sections/FeaturesSection'
import { FinalCtaSection } from './components/sections/FinalCtaSection'
import { HeroSection } from './components/sections/HeroSection'
import { HowItWorksSection } from './components/sections/HowItWorksSection'
import { ResultsPreviewSection } from './components/sections/ResultsPreviewSection'
import { TopNav } from './components/sections/TopNav'
import { TrustSection } from './components/sections/TrustSection'
import { exampleAnalysis } from './data/exampleAnalysis'
import type { ResumeAnalysisPayload, ResumeAnalysisResult } from './types/resume'

function App() {
  const [result, setResult] = useState<ResumeAnalysisResult | null>(null)
  const [submitError, setSubmitError] = useState<string | null>(null)

  const analyseMutation = useMutation({
    mutationFn: (payload: ResumeAnalysisPayload) => analyseResume(payload),
    onSuccess: (data) => {
      setSubmitError(null)
      setResult(data)
      document.getElementById('analysis-preview')?.scrollIntoView({ behavior: 'smooth', block: 'start' })
    },
    onError: (error: Error) => {
      setResult(null)
      setSubmitError(error.message)
      document.getElementById('analysis-preview')?.scrollIntoView({ behavior: 'smooth', block: 'start' })
    },
  })

  const handleSubmit = async (payload: ResumeAnalysisPayload) => {
    await analyseMutation.mutateAsync(payload)
  }

  const scrollTo = (id: string) => {
    document.getElementById(id)?.scrollIntoView({ behavior: 'smooth', block: 'start' })
  }

  return (
    <div className="relative overflow-hidden bg-slate-950">
      <div className="pointer-events-none absolute inset-x-0 top-0 h-[420px] bg-gradient-to-b from-brand-900/30 via-transparent to-transparent" />
      <TopNav onPrimaryClick={() => scrollTo('upload-card')} />

      <main className="relative">
        <HeroSection
          onSubmit={handleSubmit}
          isSubmitting={analyseMutation.isPending}
          submitError={submitError}
          onSeePreview={() => scrollTo('analysis-preview')}
          onSeeFeatures={() => scrollTo('features')}
        />
        <FeaturesSection />
        <HowItWorksSection />
        <ResultsPreviewSection
          result={result}
          isLoading={analyseMutation.isPending}
          errorMessage={submitError}
          previewResult={exampleAnalysis}
        />
        <TrustSection />
        <FinalCtaSection onPrimaryClick={() => scrollTo('upload-card')} />
      </main>
    </div>
  )
}

export default App
