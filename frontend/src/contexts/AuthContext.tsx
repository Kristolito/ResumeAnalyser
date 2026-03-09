import { createContext, useContext, useEffect, useMemo, useState, type ReactNode } from 'react'
import * as authApi from '../services/authApi'
import { clearAccessToken, getAccessToken, setAccessToken } from '../services/authStorage'
import type { AuthUser, LoginRequest, RegisterRequest } from '../types/auth'

interface AuthContextValue {
  user: AuthUser | null
  isAuthenticated: boolean
  isInitializing: boolean
  login: (payload: LoginRequest) => Promise<void>
  register: (payload: RegisterRequest) => Promise<void>
  logout: () => Promise<void>
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(null)
  const [isInitializing, setIsInitializing] = useState(true)

  useEffect(() => {
    const token = getAccessToken()
    if (!token) {
      setIsInitializing(false)
      return
    }

    void authApi
      .getMe()
      .then((me) => setUser(me))
      .catch(() => {
        clearAccessToken()
        setUser(null)
      })
      .finally(() => setIsInitializing(false))
  }, [])

  const login = async (payload: LoginRequest) => {
    const response = await authApi.login(payload)
    setAccessToken(response.accessToken)
    setUser(response.user)
  }

  const register = async (payload: RegisterRequest) => {
    const response = await authApi.register(payload)
    setAccessToken(response.accessToken)
    setUser(response.user)
  }

  const logout = async () => {
    if (getAccessToken()) {
      try {
        await authApi.logout()
      } catch {
        // ignore logout API failures; local sign-out still proceeds
      }
    }

    clearAccessToken()
    setUser(null)
  }

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      isAuthenticated: Boolean(user),
      isInitializing,
      login,
      register,
      logout,
    }),
    [user, isInitializing],
  )

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider.')
  }

  return context
}
