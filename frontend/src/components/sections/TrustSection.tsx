import { Section } from '../ui/Section'

const points = [
  'Designed for job seekers aiming for clearer, role-aligned resumes.',
  'Built to help improve interview readiness through actionable feedback.',
  'Structured output makes it easier to revise resumes systematically.',
]

export function TrustSection() {
  return (
    <Section id="trust" className="py-20">
      <div className="grid gap-8 rounded-3xl border border-white/10 bg-slate-900/60 p-8 md:grid-cols-[1.1fr,1fr] md:items-center">
        <div>
          <h2 className="text-3xl font-semibold text-white sm:text-4xl">Built for confidence, not guesswork</h2>
          <p className="mt-3 text-slate-300">
            ResumeAnalyser helps you make informed changes before applications go out.
          </p>
        </div>

        <ul className="grid gap-4">
          {points.map((point) => (
            <li key={point} className="rounded-xl border border-white/10 bg-slate-950/70 p-4 text-sm text-slate-200">
              {point}
            </li>
          ))}
        </ul>
      </div>
    </Section>
  )
}
