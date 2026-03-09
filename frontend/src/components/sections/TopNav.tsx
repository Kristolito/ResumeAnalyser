import { Button } from '../ui/Button'
import { Section } from '../ui/Section'

interface TopNavProps {
  onPrimaryClick: () => void
}

export function TopNav({ onPrimaryClick }: TopNavProps) {
  return (
    <header className="sticky top-0 z-50 border-b border-white/10 bg-slate-950/80 backdrop-blur-xl">
      <Section className="py-4">
        <nav className="flex items-center justify-between">
          <a href="#" className="flex items-center gap-2 text-sm font-semibold tracking-wide text-slate-100">
            <span className="inline-flex h-7 w-7 items-center justify-center rounded-lg bg-gradient-to-br from-brand-500 to-cyan-400 text-slate-950">
              R
            </span>
            ResumeAnalyser
          </a>

          <div className="hidden items-center gap-8 text-sm text-slate-300 md:flex">
            <a href="#features" className="transition hover:text-white">
              Features
            </a>
            <a href="#how-it-works" className="transition hover:text-white">
              How it Works
            </a>
            <a href="#analysis-preview" className="transition hover:text-white">
              Preview
            </a>
            <a href="#trust" className="transition hover:text-white">
              Why ResumeAnalyser
            </a>
          </div>

          <Button variant="secondary" onClick={onPrimaryClick} className="text-xs sm:text-sm">
            Analyse My Resume
          </Button>
        </nav>
      </Section>
    </header>
  )
}
