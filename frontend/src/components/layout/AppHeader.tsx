import { Link, useLocation } from 'react-router-dom'
import { Section } from '../ui/Section'

export function AppHeader() {
  const location = useLocation()
  const isHistory = location.pathname === '/history'

  return (
    <header className="sticky top-0 z-50 border-b border-white/10 bg-slate-950/80 backdrop-blur-xl">
      <Section className="py-4">
        <nav className="flex items-center justify-between">
          <Link to="/" className="flex items-center gap-2 text-sm font-semibold tracking-wide text-slate-100">
            <span className="inline-flex h-7 w-7 items-center justify-center rounded-lg bg-gradient-to-br from-brand-500 to-cyan-400 text-slate-950">
              R
            </span>
            ResumeAnalyser
          </Link>

          <div className="flex items-center gap-4 text-sm text-slate-300">
            <Link className={`transition hover:text-white ${!isHistory ? 'text-white' : ''}`} to="/">
              Analyse
            </Link>
            <Link className={`transition hover:text-white ${isHistory ? 'text-white' : ''}`} to="/history">
              History
            </Link>
          </div>
        </nav>
      </Section>
    </header>
  )
}
