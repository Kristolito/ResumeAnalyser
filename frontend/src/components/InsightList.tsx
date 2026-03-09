interface InsightListProps {
  title: string
  items: string[]
}

export function InsightList({ title, items }: InsightListProps) {
  return (
    <article className="insight-group">
      <h3>{title}</h3>
      {items.length === 0 ? (
        <p className="subtle">No items returned.</p>
      ) : (
        <ul>
          {items.map((item) => (
            <li key={item}>{item}</li>
          ))}
        </ul>
      )}
    </article>
  )
}
