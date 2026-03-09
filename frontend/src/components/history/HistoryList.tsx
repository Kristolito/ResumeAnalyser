import type { ResumeAnalysisHistoryItem } from '../../types/history'

interface HistoryListProps {
  items: ResumeAnalysisHistoryItem[]
  selectedId: string | null
  onSelect: (id: string) => void
}

export function HistoryList({ items, selectedId, onSelect }: HistoryListProps) {
  return (
    <div className="space-y-3">
      {items.map((item) => {
        const isSelected = selectedId === item.id
        return (
          <button
            key={item.id}
            type="button"
            onClick={() => onSelect(item.id)}
            className={`w-full rounded-2xl border p-4 text-left transition ${
              isSelected
                ? 'border-cyan-300/60 bg-slate-800/70'
                : 'border-white/10 bg-slate-900/60 hover:border-white/30'
            }`}
          >
            <div className="flex items-center justify-between gap-3">
              <p className="text-sm font-semibold text-white">{item.targetJobTitle}</p>
              <p className="text-xs text-slate-400">{new Date(item.createdAt).toLocaleDateString()}</p>
            </div>
            <p className="mt-2 truncate text-xs text-slate-400">{item.originalFileName}</p>
            <div className="mt-3 flex gap-3 text-xs">
              <span className="rounded-lg bg-brand-500/20 px-2 py-1 text-brand-100">Overall {item.overallScore}</span>
              <span className="rounded-lg bg-cyan-500/20 px-2 py-1 text-cyan-100">ATS {item.atsScore}</span>
            </div>
          </button>
        )
      })}
    </div>
  )
}
