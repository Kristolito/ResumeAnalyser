import { env } from '../config/env'
import { getAccessToken } from './authStorage'

interface ApiRequestOptions extends RequestInit {
  auth?: boolean
}

async function parseError(response: Response): Promise<string> {
  const text = await response.text()
  return text || `Request failed with status ${response.status}.`
}

export async function apiRequest<T>(path: string, options: ApiRequestOptions = {}): Promise<T> {
  const headers = new Headers(options.headers)
  const shouldAttachAuth = options.auth ?? true

  if (shouldAttachAuth) {
    const token = getAccessToken()
    if (!token) {
      throw new Error('You need to sign in first.')
    }

    headers.set('Authorization', `Bearer ${token}`)
  }

  const response = await fetch(`${env.apiBaseUrl}${path}`, {
    ...options,
    headers,
  })

  if (!response.ok) {
    if (response.status === 401) {
      throw new Error('Your session has expired. Please sign in again.')
    }

    throw new Error(await parseError(response))
  }

  if (response.status === 204) {
    return undefined as T
  }

  return (await response.json()) as T
}
