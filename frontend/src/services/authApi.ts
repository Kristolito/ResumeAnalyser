import { apiRequest } from './apiClient'
import type { AuthResponse, AuthUser, LoginRequest, RegisterRequest } from '../types/auth'

export function register(payload: RegisterRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/api/auth/register', {
    method: 'POST',
    auth: false,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  })
}

export function login(payload: LoginRequest): Promise<AuthResponse> {
  return apiRequest<AuthResponse>('/api/auth/login', {
    method: 'POST',
    auth: false,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  })
}

export function getMe(): Promise<AuthUser> {
  return apiRequest<AuthUser>('/api/auth/me', {
    method: 'GET',
  })
}

export function logout(): Promise<{ message: string }> {
  return apiRequest<{ message: string }>('/api/auth/logout', {
    method: 'POST',
  })
}
