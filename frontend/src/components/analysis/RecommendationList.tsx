interface RecommendationListProps {
  recommendations: string[]
}

export function RecommendationList({ recommendations }: RecommendationListProps) {
  if (recommendations.length === 0) {
    return <p className="text-sm text-slate-400">No recommendations returned.</p>
  }

  return (
    <ol className="mt-3 space-y-2">
      {recommendations.map((recommendation, index) => (
        <li key={recommendation} className="flex gap-3 rounded-lg border border-white/10 bg-slate-950/60 px-3 py-2 text-sm text-slate-200">
          <span className="mt-[2px] inline-flex h-5 w-5 shrink-0 items-center justify-center rounded-full bg-brand-500/20 text-xs font-semibold text-brand-100">
            {index + 1}
          </span>
          <span>{recommendation}</span>
        </li>
      ))}
    </ol>
  )
}
