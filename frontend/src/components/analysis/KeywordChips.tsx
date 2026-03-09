interface KeywordChipsProps {
  keywords: string[]
}

export function KeywordChips({ keywords }: KeywordChipsProps) {
  if (keywords.length === 0) {
    return <p className="text-sm text-slate-400">No missing keywords detected.</p>
  }

  return (
    <div className="mt-3 flex flex-wrap gap-2">
      {keywords.map((keyword) => (
        <span
          key={keyword}
          className="rounded-full border border-cyan-300/30 bg-cyan-400/10 px-3 py-1 text-xs font-medium text-cyan-100"
        >
          {keyword}
        </span>
      ))}
    </div>
  )
}
