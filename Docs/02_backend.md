# Backend Design (.NET)

Project structure:

/src /Controllers ResumeController.cs /Services PdfTextExtractor.cs
AiResumeAnalysisService.cs /Models ResumeAnalysisRequest.cs
ResumeAnalysisResult.cs /Infrastructure OpenAIClient.cs

Main endpoint:

POST /api/resume/analyse

Steps:

1.  Receive PDF via multipart/form-data
2.  Validate file type and size
3.  Extract text from PDF
4.  Build AI prompt
5.  Send request to AI
6.  Parse JSON response
7.  Return result to frontend
