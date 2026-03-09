import type { ResumeAnalysisResponse } from '../../types/resumeAnalysis'

interface ScoreBreakdownProps {
  breakdown?: ResumeAnalysisResponse['scoreBreakdown']
}

const defaultBreakdown = {
  structure: 0,
  keywordAlignment: 0,
  skillsCoverage: 0,
  achievementEvidence: 0,
  readability: 0,
}

const labels: Array<{ key: keyof typeof defaultBreakdown; label: string }> = [
  { key: 'structure', label: 'Structure' },
  { key: 'keywordAlignment', label: 'Keyword Alignment' },
  { key: 'skillsCoverage', label: 'Skills Coverage' },
  { key: 'achievementEvidence', label: 'Achievement Evidence' },
  { key: 'readability', label: 'Readability' },
]

export function ScoreBreakdown({ breakdown }: ScoreBreakdownProps) {
  const data = breakdown ?? defaultBreakdown

  return (
    <section className="rounded-2xl border border-white/10 bg-slate-900/60 p-5">
      <h4 className="text-sm font-semibold uppercase tracking-wider text-slate-300">Score Breakdown</h4>
      <div className="mt-4 space-y-3">
        {labels.map(({ key, label }) => {
          const value = data[key]
          return (
            <div key={key} className="space-y-1">
              <div className="flex items-center justify-between text-sm">
                <span className="text-slate-300">{label}</span>
                <span className="font-semibold text-white">{value}</span>
              </div>
              <div className="h-2 overflow-hidden rounded-full bg-slate-800">
                <div
                  className="h-full rounded-full bg-gradient-to-r from-brand-400 to-cyan-400"
                  style={{ width: `${Math.max(0, Math.min(100, value))}%` }}
                />
              </div>
            </div>
          )
        })}
      </div>
    </section>
  )
}
