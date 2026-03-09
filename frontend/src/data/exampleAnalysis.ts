import type { ResumeAnalysisResult } from '../types/resume'

export const exampleAnalysis: ResumeAnalysisResult = {
  overallScore: 82,
  atsScore: 78,
  candidateSummary:
    'The resume shows relevant backend experience and measurable impact, but stronger role-specific phrasing would improve alignment with the target position.',
  strengths: [
    'Clear experience progression across backend-focused roles',
    'Good use of metrics in project and delivery outcomes',
    'Solid technical stack alignment for API and data services',
  ],
  weaknesses: [
    'Profile summary is broad and could be more role-targeted',
    'Some bullet points describe tasks more than outcomes',
  ],
  missingKeywords: ['system design', 'stakeholder communication', 'mentoring'],
  recommendations: [
    'Tailor the summary to the target role and highlight relevant domain impact first.',
    'Add two to three quantified achievements in recent experience sections.',
    'Integrate missing keywords naturally into project descriptions.',
  ],
}
