import { Badge } from '../ui/Badge'
import { Button } from '../ui/Button'
import { Section } from '../ui/Section'
import { ResumeUploadForm } from '../ResumeUploadForm'
import type { ResumeAnalysisRequest } from '../../types/resumeAnalysis'

interface HeroSectionProps {
  onSubmit: (payload: ResumeAnalysisRequest) => Promise<void>
  isSubmitting: boolean
  submitError: string | null
  onSeePreview: () => void
  onSeeFeatures: () => void
}

const benefitChips = ['ResumeAnalyser readiness checks', 'Role-specific keyword gaps', 'Structured recommendations']

export function HeroSection({
  onSubmit,
  isSubmitting,
  submitError,
  onSeePreview,
  onSeeFeatures,
}: HeroSectionProps) {
  return (
    <Section className="relative pt-12 sm:pt-16">
      <div className="absolute -left-10 top-20 h-44 w-44 rounded-full bg-brand-500/20 blur-3xl" />
      <div className="absolute -right-8 top-6 h-40 w-40 rounded-full bg-cyan-400/20 blur-3xl" />

      <div className="relative grid gap-8 lg:grid-cols-[1fr,460px]">
        <div className="pt-2">
          <Badge>AI Resume Intelligence</Badge>
          <h1 className="mt-5 max-w-2xl text-4xl font-semibold tracking-tight text-white sm:text-5xl lg:text-6xl">
            Turn resume feedback into better interview opportunities
          </h1>
          <p className="mt-5 max-w-xl text-base leading-7 text-slate-300 sm:text-lg">
            ResumeAnalyser reviews your PDF resume against the role you want, then returns clear scoring and practical
            recommendations you can act on quickly.
          </p>

          <div className="mt-7 flex flex-wrap gap-3">
            <Button onClick={onSeePreview}>See Analysis Preview</Button>
            <Button variant="secondary" onClick={onSeeFeatures}>Explore Features</Button>
          </div>

          <ul className="mt-8 grid gap-3 sm:grid-cols-3">
            {benefitChips.map((chip) => (
              <li
                key={chip}
                className="rounded-xl border border-white/10 bg-slate-900/70 px-4 py-3 text-xs font-medium text-slate-200 sm:text-sm"
              >
                {chip}
              </li>
            ))}
          </ul>
        </div>

        <div id="upload-card" className="animate-float-slow rounded-3xl border border-white/15 bg-slate-900/70 p-6 shadow-glow">
          <div className="mb-5">
            <p className="text-xs font-semibold uppercase tracking-widest text-cyan-300">Start Analysis</p>
            <h2 className="mt-2 text-2xl font-semibold text-white">Upload your resume</h2>
            <p className="mt-2 text-sm text-slate-300">
              Add your target role details and generate a structured analysis report.
            </p>
          </div>

          <ResumeUploadForm onSubmit={onSubmit} isSubmitting={isSubmitting} />

          {submitError ? (
            <div
              className="mt-4 rounded-xl border border-red-400/40 bg-red-500/10 px-4 py-3 text-sm text-red-100"
              role="alert"
            >
              {submitError}
            </div>
          ) : null}
        </div>
      </div>
    </Section>
  )
}
