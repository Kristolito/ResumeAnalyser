interface ScoreCardProps {
  title: string
  score: number
}

export function ScoreCard({ title, score }: ScoreCardProps) {
  return (
    <article className="rounded-2xl border border-white/10 bg-slate-900/70 p-5">
      <p className="text-xs uppercase tracking-wider text-slate-400">{title}</p>
      <p className="mt-2 text-3xl font-semibold text-white">{score}</p>
      <div className="mt-3 h-2 overflow-hidden rounded-full bg-slate-800">
        <div className="h-full rounded-full bg-gradient-to-r from-brand-400 to-cyan-400" style={{ width: `${score}%` }} />
      </div>
    </article>
  )
}
