# API Contract

Endpoint:

POST /api/resume/analyse

Request (multipart/form-data)

file: PDF targetJobTitle: string targetJobDescription: string notes:
optional string

Response:

{ overallScore: number, atsScore: number, candidateSummary: string,
strengths: string\[\], weaknesses: string\[\], missingKeywords:
string\[\], recommendations: string\[\] }
