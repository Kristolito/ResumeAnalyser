import { Navigate, Route, Routes } from 'react-router-dom'
import { AnalysePage } from './pages/AnalysePage'
import { HistoryPage } from './pages/HistoryPage'

function App() {
  return (
    <Routes>
      <Route path="/" element={<AnalysePage />} />
      <Route path="/history" element={<HistoryPage />} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}

export default App
