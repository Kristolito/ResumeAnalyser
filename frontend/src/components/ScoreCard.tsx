interface ScoreCardProps {
  title: string
  score: number
}

export function ScoreCard({ title, score }: ScoreCardProps) {
  return (
    <article className="score-card">
      <p className="score-title">{title}</p>
      <p className="score-value">{score}</p>
    </article>
  )
}
