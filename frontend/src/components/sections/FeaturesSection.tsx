import { Section } from '../ui/Section'

const features = [
  {
    title: 'ResumeAnalyser Compatibility Insights',
    description:
      'See how resume structure and wording align with common applicant tracking filters before you apply.',
  },
  {
    title: 'AI-Powered Resume Review',
    description:
      'Get a structured analysis across summary quality, role alignment, and experience positioning.',
  },
  {
    title: 'Keyword Gap Detection',
    description:
      'Highlight missing keywords from the target job description so you can tune your resume with intent.',
  },
  {
    title: 'Actionable Recommendations',
    description:
      'Receive practical rewrite guidance that helps you improve clarity, relevance, and impact.',
  },
]

export function FeaturesSection() {
  return (
    <Section id="features" className="py-20">
      <div className="mx-auto max-w-2xl text-center">
        <h2 className="text-3xl font-semibold text-white sm:text-4xl">Everything you need to refine your resume</h2>
        <p className="mt-4 text-slate-300">
          ResumeAnalyser turns resume feedback into a focused action plan you can use immediately.
        </p>
      </div>

      <div className="mt-10 grid gap-5 md:grid-cols-2">
        {features.map((feature) => (
          <article
            key={feature.title}
            className="rounded-2xl border border-white/10 bg-slate-900/60 p-6 shadow-lg shadow-black/20 backdrop-blur"
          >
            <h3 className="text-lg font-semibold text-white">{feature.title}</h3>
            <p className="mt-2 text-sm leading-6 text-slate-300">{feature.description}</p>
          </article>
        ))}
      </div>
    </Section>
  )
}
