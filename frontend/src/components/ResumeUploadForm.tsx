import { useState, type FormEvent } from 'react'
import type { ResumeAnalysisRequest } from '../types/resumeAnalysis'

interface ResumeUploadFormProps {
  onSubmit: (payload: ResumeAnalysisRequest) => Promise<void>
  isSubmitting: boolean
}

export function ResumeUploadForm({ onSubmit, isSubmitting }: ResumeUploadFormProps) {
  const [file, setFile] = useState<File | null>(null)
  const [targetJobTitle, setTargetJobTitle] = useState('')
  const [targetJobDescription, setTargetJobDescription] = useState('')
  const [notes, setNotes] = useState('')
  const [formError, setFormError] = useState<string | null>(null)

  const handleFormSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setFormError(null)

    if (!file) {
      setFormError('Please select a PDF resume file.')
      return
    }

    if (file.type !== 'application/pdf') {
      setFormError('Only PDF files are allowed.')
      return
    }

    if (!targetJobTitle.trim() || !targetJobDescription.trim()) {
      setFormError('Target job title and description are required.')
      return
    }

    await onSubmit({
      file,
      targetJobTitle: targetJobTitle.trim(),
      targetJobDescription: targetJobDescription.trim(),
      notes: notes.trim() || undefined,
    })
  }

  return (
    <form className="space-y-4" onSubmit={handleFormSubmit}>
      <label className="block space-y-2 text-sm text-slate-200">
        <span className="font-medium">Resume PDF</span>
        <input
          type="file"
          accept="application/pdf,.pdf"
          className="block w-full rounded-xl border border-dashed border-white/20 bg-slate-950/60 px-3 py-3 text-sm text-slate-200 file:mr-4 file:rounded-lg file:border-0 file:bg-brand-500/30 file:px-3 file:py-2 file:text-xs file:font-semibold file:text-brand-100 focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-400/30"
          onChange={(event) => {
            setFile(event.target.files?.[0] ?? null)
          }}
          required
        />
        <p className="text-xs text-slate-400">{file ? `Selected: ${file.name}` : 'Only PDF files are supported.'}</p>
      </label>

      <label className="block space-y-2 text-sm text-slate-200">
        <span className="font-medium">Target Job Title</span>
        <input
          type="text"
          value={targetJobTitle}
          onChange={(event) => setTargetJobTitle(event.target.value)}
          placeholder="e.g., Senior Backend Engineer"
          className="w-full rounded-xl border border-white/15 bg-slate-950/60 px-3 py-2.5 text-sm text-slate-100 placeholder:text-slate-400 focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-400/30"
          required
        />
      </label>

      <label className="block space-y-2 text-sm text-slate-200">
        <span className="font-medium">Target Job Description</span>
        <textarea
          value={targetJobDescription}
          onChange={(event) => setTargetJobDescription(event.target.value)}
          rows={6}
          placeholder="Paste the job description here..."
          className="w-full rounded-xl border border-white/15 bg-slate-950/60 px-3 py-2.5 text-sm text-slate-100 placeholder:text-slate-400 focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-400/30"
          required
        />
      </label>

      <label className="block space-y-2 text-sm text-slate-200">
        <span className="font-medium">Optional Notes</span>
        <textarea
          value={notes}
          onChange={(event) => setNotes(event.target.value)}
          rows={4}
          placeholder="Add anything else the analysis should consider..."
          className="w-full rounded-xl border border-white/15 bg-slate-950/60 px-3 py-2.5 text-sm text-slate-100 placeholder:text-slate-400 focus:border-brand-300 focus:outline-none focus:ring-2 focus:ring-brand-400/30"
        />
      </label>

      {formError ? (
        <div className="rounded-xl border border-red-400/40 bg-red-500/10 px-4 py-3 text-sm text-red-100" role="alert">
          {formError}
        </div>
      ) : null}

      <button
        className="inline-flex w-full items-center justify-center rounded-xl bg-gradient-to-r from-brand-500 via-cyan-400 to-emerald-400 px-4 py-3 text-sm font-semibold text-slate-950 shadow-glow transition hover:brightness-110 disabled:cursor-not-allowed disabled:opacity-60"
        type="submit"
        disabled={isSubmitting}
      >
        {isSubmitting ? 'Analysing...' : 'Analyse Resume'}
      </button>
    </form>
  )
}
