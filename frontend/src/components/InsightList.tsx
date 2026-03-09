interface InsightListProps {
  title: string
  items: string[]
}

export function InsightList({ title, items }: InsightListProps) {
  return (
    <article className="rounded-2xl border border-white/10 bg-slate-900/60 p-5">
      <h3 className="text-base font-semibold text-white">{title}</h3>
      {items.length === 0 ? (
        <p className="mt-2 text-sm text-slate-400">No items returned.</p>
      ) : (
        <ul className="mt-3 space-y-2">
          {items.map((item) => (
            <li key={item} className="rounded-lg border border-white/10 bg-slate-950/60 px-3 py-2 text-sm text-slate-200">
              {item}
            </li>
          ))}
        </ul>
      )}
    </article>
  )
}
