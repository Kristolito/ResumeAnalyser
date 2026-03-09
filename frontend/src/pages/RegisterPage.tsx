import { useState, type FormEvent } from 'react'
import { Link, Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'

export function RegisterPage() {
  const { register, isAuthenticated } = useAuth()
  const navigate = useNavigate()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  if (isAuthenticated) {
    return <Navigate to="/" replace />
  }

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setError(null)

    if (password !== confirmPassword) {
      setError('Passwords do not match.')
      return
    }

    setIsSubmitting(true)
    try {
      await register({ email: email.trim(), password })
      navigate('/', { replace: true })
    } catch (submitError) {
      setError(submitError instanceof Error ? submitError.message : 'Registration failed. Please try again.')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-950 px-4">
      <div className="w-full max-w-md rounded-2xl border border-white/10 bg-slate-900/70 p-6">
        <h1 className="text-2xl font-semibold text-white">Create your account</h1>
        <p className="mt-1 text-sm text-slate-300">Start tracking private analysis history in ResumeAnalyser.</p>

        <form className="mt-6 space-y-4" onSubmit={handleSubmit}>
          <label className="block space-y-2 text-sm text-slate-200">
            <span>Email</span>
            <input
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              className="w-full rounded-xl border border-white/15 bg-slate-950/60 px-3 py-2.5 text-sm text-slate-100 placeholder:text-slate-400 focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-400/30"
              required
            />
          </label>

          <label className="block space-y-2 text-sm text-slate-200">
            <span>Password</span>
            <input
              type="password"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              className="w-full rounded-xl border border-white/15 bg-slate-950/60 px-3 py-2.5 text-sm text-slate-100 placeholder:text-slate-400 focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-400/30"
              required
              minLength={8}
            />
          </label>

          <label className="block space-y-2 text-sm text-slate-200">
            <span>Confirm Password</span>
            <input
              type="password"
              value={confirmPassword}
              onChange={(event) => setConfirmPassword(event.target.value)}
              className="w-full rounded-xl border border-white/15 bg-slate-950/60 px-3 py-2.5 text-sm text-slate-100 placeholder:text-slate-400 focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-400/30"
              required
              minLength={8}
            />
          </label>

          {error ? <div className="rounded-xl border border-red-400/40 bg-red-500/10 p-3 text-sm text-red-100">{error}</div> : null}

          <button
            type="submit"
            disabled={isSubmitting}
            className="inline-flex w-full items-center justify-center rounded-xl bg-gradient-to-r from-brand-500 via-cyan-400 to-emerald-400 px-4 py-3 text-sm font-semibold text-slate-950 shadow-glow transition hover:brightness-110 disabled:cursor-not-allowed disabled:opacity-60"
          >
            {isSubmitting ? 'Creating account...' : 'Create Account'}
          </button>
        </form>

        <p className="mt-5 text-sm text-slate-300">
          Already have an account?{' '}
          <Link to="/login" className="font-semibold text-cyan-200 hover:text-white">
            Sign in
          </Link>
        </p>
      </div>
    </div>
  )
}
