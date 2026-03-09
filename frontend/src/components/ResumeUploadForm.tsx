import { useState, type FormEvent } from 'react'
import type { ResumeAnalysisPayload } from '../types/resume'

interface ResumeUploadFormProps {
  onSubmit: (payload: ResumeAnalysisPayload) => Promise<void>
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
    <form className="form-grid" onSubmit={handleFormSubmit}>
      <label className="field">
        Resume PDF
        <input
          type="file"
          accept="application/pdf,.pdf"
          onChange={(event) => {
            setFile(event.target.files?.[0] ?? null)
          }}
          required
        />
      </label>

      <label className="field">
        Target Job Title
        <input
          type="text"
          value={targetJobTitle}
          onChange={(event) => setTargetJobTitle(event.target.value)}
          placeholder="e.g., Senior Backend Engineer"
          required
        />
      </label>

      <label className="field">
        Target Job Description
        <textarea
          value={targetJobDescription}
          onChange={(event) => setTargetJobDescription(event.target.value)}
          rows={6}
          placeholder="Paste the job description here..."
          required
        />
      </label>

      <label className="field">
        Optional Notes
        <textarea
          value={notes}
          onChange={(event) => setNotes(event.target.value)}
          rows={4}
          placeholder="Add anything else the analysis should consider..."
        />
      </label>

      {formError ? (
        <div className="error-banner" role="alert">
          {formError}
        </div>
      ) : null}

      <button className="submit-button" type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Analysing...' : 'Analyse Resume'}
      </button>
    </form>
  )
}
