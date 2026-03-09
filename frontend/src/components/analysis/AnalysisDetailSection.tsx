import type { ReactNode } from 'react'

interface AnalysisDetailSectionProps {
  title: string
  children: ReactNode
}

export function AnalysisDetailSection({ title, children }: AnalysisDetailSectionProps) {
  return (
    <section className="rounded-2xl border border-white/10 bg-slate-900/60 p-5">
      <h4 className="text-sm font-semibold uppercase tracking-wider text-slate-300">{title}</h4>
      <div className="mt-2">{children}</div>
    </section>
  )
}
