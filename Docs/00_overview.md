# ResumeAnalyser -- Project Overview

ResumeAnalyser is a web application that allows users to upload their
resume (PDF) and receive AI‑powered feedback.

The goal of the system is to: - Analyse resumes against job
descriptions - Estimate ATS compatibility - Identify missing keywords -
Suggest improvements - Provide a structured report for the user

Tech stack: Frontend: React + TypeScript + Vite + Tailwind Backend:
ASP.NET Core Web API (.NET) Database: MySQL or PostgreSQL AI: OpenAI API

High level flow:

User uploads PDF → Backend extracts text → Backend sends resume + job
description to AI → AI returns structured JSON analysis → Frontend
renders the results
