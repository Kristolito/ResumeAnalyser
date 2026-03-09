import { Button } from '../ui/Button'
import { Section } from '../ui/Section'

interface FinalCtaSectionProps {
  onPrimaryClick: () => void
}

export function FinalCtaSection({ onPrimaryClick }: FinalCtaSectionProps) {
  return (
    <Section className="pb-20 pt-8">
      <div className="rounded-3xl border border-brand-300/30 bg-gradient-to-r from-brand-700/40 via-slate-900 to-cyan-700/30 p-8 text-center sm:p-12">
        <h2 className="text-3xl font-semibold text-white sm:text-4xl">
          Ready to improve your resume for your next role?
        </h2>
        <p className="mx-auto mt-3 max-w-2xl text-slate-200">
          Upload your resume, add a target role, and get a focused analysis in minutes.
        </p>
        <Button onClick={onPrimaryClick} className="mt-7">
          Start Free Analysis
        </Button>
      </div>
    </Section>
  )
}
