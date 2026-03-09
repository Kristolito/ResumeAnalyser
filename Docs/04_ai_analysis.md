# AI Analysis Layer

The AI service receives:

Resume text Target job title Job description Country or region

The AI is instructed to return structured JSON.

Example output:

{ "candidateSummary": "...", "overallScore": 80, "atsScore": 75,
"strengths": \[\], "weaknesses": \[\], "missingKeywords": \[\],
"recommendations": \[\] }

Prompt should instruct the AI to return valid JSON only.
