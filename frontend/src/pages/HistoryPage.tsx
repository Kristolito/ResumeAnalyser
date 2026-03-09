import { useQuery } from '@tanstack/react-query'
import { useEffect, useState } from 'react'
import { AppHeader } from '../components/layout/AppHeader'
import { HistoryDetailPanel } from '../components/history/HistoryDetailPanel'
import { HistoryList } from '../components/history/HistoryList'
import { Section } from '../components/ui/Section'
import { getAnalysisHistory, getAnalysisHistoryById } from '../services/historyApi'

export function HistoryPage() {
  const [selectedId, setSelectedId] = useState<string | null>(null)

  const historyQuery = useQuery({
    queryKey: ['analysis-history'],
    queryFn: getAnalysisHistory,
  })

  useEffect(() => {
    if (!selectedId && historyQuery.data && historyQuery.data.length > 0) {
      setSelectedId(historyQuery.data[0].id)
    }
  }, [historyQuery.data, selectedId])

  const detailsQuery = useQuery({
    queryKey: ['analysis-history', selectedId],
    queryFn: () => getAnalysisHistoryById(selectedId!),
    enabled: Boolean(selectedId),
  })

  return (
    <div className="min-h-screen bg-slate-950">
      <AppHeader />

      <Section className="py-12">
        <div className="mb-8">
          <h1 className="text-3xl font-semibold text-white sm:text-4xl">Analysis History</h1>
          <p className="mt-2 text-slate-300">Review your previous resume analyses and inspect full scoring details.</p>
        </div>

        {historyQuery.isLoading ? (
          <div className="rounded-2xl border border-white/10 bg-slate-900/60 p-6 text-slate-300">Loading history...</div>
        ) : null}

        {historyQuery.error ? (
          <div className="rounded-2xl border border-red-400/40 bg-red-500/10 p-6 text-sm text-red-100">
            {(historyQuery.error as Error).message}
          </div>
        ) : null}

        {!historyQuery.isLoading && !historyQuery.error && historyQuery.data?.length === 0 ? (
          <div className="rounded-2xl border border-white/10 bg-slate-900/60 p-6 text-slate-300">
            No analyses found yet. Run an analysis from the main page and your history will appear here.
          </div>
        ) : null}

        {historyQuery.data && historyQuery.data.length > 0 ? (
          <div className="grid gap-6 lg:grid-cols-[360px,1fr]">
            <HistoryList items={historyQuery.data} selectedId={selectedId} onSelect={setSelectedId} />
            <HistoryDetailPanel
              item={detailsQuery.data ?? null}
              isLoading={detailsQuery.isLoading}
              error={detailsQuery.error ? (detailsQuery.error as Error).message : null}
            />
          </div>
        ) : null}
      </Section>
    </div>
  )
}
