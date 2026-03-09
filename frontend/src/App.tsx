import { useMutation } from '@tanstack/react-query'
import { useState } from 'react'
import { analyseResume } from './api/resumeApi'
import { AnalysisResults } from './components/AnalysisResults'
import { ResumeUploadForm } from './components/ResumeUploadForm'
import type { ResumeAnalysisPayload, ResumeAnalysisResult } from './types/resume'

function App() {
  const [result, setResult] = useState<ResumeAnalysisResult | null>(null)
  const [submitError, setSubmitError] = useState<string | null>(null)

  const analyseMutation = useMutation({
    mutationFn: (payload: ResumeAnalysisPayload) => analyseResume(payload),
    onSuccess: (data) => {
      setSubmitError(null)
      setResult(data)
    },
    onError: (error: Error) => {
      setResult(null)
      setSubmitError(error.message)
    },
  })

  const handleSubmit = async (payload: ResumeAnalysisPayload) => {
    await analyseMutation.mutateAsync(payload)
  }

  return (
    <main className="app-shell">
      <section className="panel">
        <div className="panel-header">
          <p className="eyebrow">ResumeAnalyser</p>
          <h1>Phase 1 Resume Analysis</h1>
          <p className="subtle">
            Upload a PDF resume, provide job context, and receive a structured analysis response.
          </p>
        </div>

        <ResumeUploadForm onSubmit={handleSubmit} isSubmitting={analyseMutation.isPending} />

        {submitError ? (
          <div className="error-banner" role="alert">
            {submitError}
          </div>
        ) : null}
      </section>

      <section className="panel">
        <AnalysisResults result={result} isLoading={analyseMutation.isPending} />
      </section>
    </main>
  )
}

export default App
