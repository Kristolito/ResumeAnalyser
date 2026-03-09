# ResumeAnalyser

ResumeAnalyser is a full-stack web app that helps you review a resume against a target role.

You upload a PDF resume, add job context (title + description + optional notes), and get a structured analysis with:
- overall score
- ATS score
- summary
- strengths and weaknesses
- missing keywords
- concrete recommendations

The app currently uses a deterministic rule-based engine (no OpenAI yet), so analysis is consistent and explainable.

## Tech Stack

- Frontend: React + TypeScript + Vite + Tailwind
- Backend: ASP.NET Core Web API (.NET)
- Database: MySQL
- ORM: Entity Framework Core + Pomelo MySQL provider
- PDF extraction: PdfPig
- State/API client (frontend): TanStack Query

## What’s Implemented

- Resume upload (`multipart/form-data`) to backend
- PDF validation (required, PDF-only, max 5MB)
- PDF text extraction from all pages
- Rule-based analysis engine with:
- section detection
- keyword alignment vs target role
- technical skill detection (languages/frameworks/cloud/db/devops/tooling)
- achievement signal detection (metrics, %, currency, impact/action language)
- weighted scoring model (`overallScore`, `atsScore`)
- Analysis history persistence
- History API endpoints + frontend `/history` page

## Project Structure

```text
ResumeAnalyser/
├─ backend/         # ASP.NET Core API
├─ backend.tests/   # xUnit tests for rule-based analysis helpers
├─ frontend/        # React app
└─ Docs/            # planning docs
```

## Quick Start

### 1) Prerequisites

- .NET SDK 10
- Node.js 20+
- MySQL 8+

### 2) Backend

```powershell
cd "C:\Users\krist\Projects\ResumeAnalyser\backend"
dotnet restore
dotnet run
```

Default local API URL:
- `http://localhost:5112`

Configure in:
- `backend/appsettings.Development.json`

### 3) Frontend

```powershell
cd "C:\Users\krist\Projects\ResumeAnalyser\frontend"
npm install
npm run dev -- --host --port 5173
```

Open:
- `http://localhost:5173`

### 4) Frontend API Base URL (optional)

Create `frontend/.env` from `.env.example` if you need a custom backend URL:

```env
VITE_API_BASE_URL=http://localhost:5112
```

## API Overview

- `POST /api/resume/analyse`
- `GET /api/resume/analyses`
- `GET /api/resume/analyses/{id}`

## Running Tests

```powershell
cd "C:\Users\krist\Projects\ResumeAnalyser\backend.tests"
dotnet test
```

## Current Limitations

- Analysis is rule-based only (OpenAI integration intentionally not implemented yet).
- OCR for scanned/image-only PDFs is not implemented yet.
- Authentication is not implemented yet.

## Next Logical Milestones

- Add optional AI-assisted analysis strategy behind existing service abstraction
- Add better integration tests for upload + analysis + history flows
- Add auth and per-user history isolation
