import type { ResumeAnalysisHistoryDetail } from '../../types/history'

interface HistoryDetailPanelProps {
  item: ResumeAnalysisHistoryDetail | null
  isLoading: boolean
  error: string | null
}

function ListBlock({ title, items }: { title: string; items: string[] }) {
  return (
    <div className="rounded-xl border border-white/10 bg-slate-950/60 p-4">
      <h4 className="text-sm font-semibold text-white">{title}</h4>
      <ul className="mt-2 space-y-2 text-sm text-slate-300">
        {items.map((entry) => (
          <li key={entry} className="rounded-lg border border-white/10 bg-slate-900/60 px-3 py-2">
            {entry}
          </li>
        ))}
      </ul>
    </div>
  )
}

export function HistoryDetailPanel({ item, isLoading, error }: HistoryDetailPanelProps) {
  if (isLoading) {
    return <div className="rounded-2xl border border-white/10 bg-slate-900/60 p-6 text-slate-300">Loading analysis details...</div>
  }

  if (error) {
    return (
      <div className="rounded-2xl border border-red-400/40 bg-red-500/10 p-6 text-sm text-red-100">
        {error}
      </div>
    )
  }

  if (!item) {
    return (
      <div className="rounded-2xl border border-white/10 bg-slate-900/60 p-6 text-slate-300">
        Select an analysis to view full details.
      </div>
    )
  }

  return (
    <article className="rounded-2xl border border-white/10 bg-slate-900/60 p-6">
      <div className="flex flex-wrap items-center justify-between gap-3">
        <div>
          <h3 className="text-xl font-semibold text-white">{item.targetJobTitle}</h3>
          <p className="mt-1 text-xs text-slate-400">
            {item.originalFileName} · {new Date(item.createdAt).toLocaleString()}
          </p>
        </div>
        <div className="flex gap-2 text-xs">
          <span className="rounded-lg bg-brand-500/20 px-2 py-1 text-brand-100">Overall {item.overallScore}</span>
          <span className="rounded-lg bg-cyan-500/20 px-2 py-1 text-cyan-100">ATS {item.atsScore}</span>
        </div>
      </div>

      <div className="mt-4 rounded-xl border border-white/10 bg-slate-950/60 p-4">
        <h4 className="text-sm font-semibold text-white">Candidate Summary</h4>
        <p className="mt-2 text-sm text-slate-300">{item.candidateSummary}</p>
      </div>

      <div className="mt-4 grid gap-4 lg:grid-cols-2">
        <ListBlock title="Strengths" items={item.strengths} />
        <ListBlock title="Weaknesses" items={item.weaknesses} />
        <ListBlock title="Missing Keywords" items={item.missingKeywords} />
        <ListBlock title="Recommendations" items={item.recommendations} />
      </div>

      <div className="mt-4 rounded-xl border border-white/10 bg-slate-950/60 p-4 text-sm text-slate-300">
        <p>
          <span className="font-semibold text-white">Target Job Description:</span> {item.targetJobDescription}
        </p>
        {item.notes ? (
          <p className="mt-2">
            <span className="font-semibold text-white">Notes:</span> {item.notes}
          </p>
        ) : null}
      </div>
    </article>
  )
}
