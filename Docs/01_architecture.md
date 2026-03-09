# Architecture

The system follows a clean layered architecture.

Frontend React SPA responsible for UI and file uploads.

Backend API ASP.NET Core Web API that handles: - File upload - PDF
processing - AI communication - Business logic

AI Service Layer Responsible for prompt construction and sending
requests to the AI model.

Database Layer Stores: - Users - Resume uploads - Analysis results -
History of analyses

Infrastructure Storage for uploaded files (local dev, cloud in
production).
