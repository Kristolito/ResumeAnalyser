import { Section } from '../ui/Section'

const steps = [
  {
    title: 'Upload your resume',
    description: 'Add your PDF resume securely from your device.',
  },
  {
    title: 'Add target role context',
    description: 'Provide the role title and job description you want to optimize for.',
  },
  {
    title: 'AI runs the analysis',
    description: 'Resume content and role context are evaluated together for relevance and ResumeAnalyser fit.',
  },
  {
    title: 'Apply recommendations',
    description: 'Use strengths, weaknesses, and missing keyword guidance to improve your resume.',
  },
]

export function HowItWorksSection() {
  return (
    <Section id="how-it-works" className="py-20">
      <div className="rounded-3xl border border-white/10 bg-gradient-to-br from-slate-900 to-slate-950 p-8 sm:p-10">
        <h2 className="text-3xl font-semibold text-white sm:text-4xl">How it works</h2>
        <p className="mt-3 max-w-2xl text-slate-300">A fast workflow designed for focused resume improvements.</p>

        <div className="mt-10 grid gap-5 md:grid-cols-2">
          {steps.map((step, index) => (
            <article
              key={step.title}
              className="rounded-2xl border border-white/10 bg-slate-900/60 p-5 transition hover:border-cyan-300/40"
            >
              <p className="text-xs font-semibold uppercase tracking-widest text-cyan-300">Step {index + 1}</p>
              <h3 className="mt-2 text-lg font-semibold text-white">{step.title}</h3>
              <p className="mt-2 text-sm leading-6 text-slate-300">{step.description}</p>
            </article>
          ))}
        </div>
      </div>
    </Section>
  )
}
