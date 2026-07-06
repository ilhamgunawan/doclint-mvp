import axios from 'axios'
import type { LintReport, ApiError } from '../types'

const client = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
})

export class ApiRequestError extends Error {
  code: string

  constructor(code: string, message: string) {
    super(message)
    this.code = code
  }
}

export async function lintDocument(file: File): Promise<LintReport> {
  const formData = new FormData()
  formData.append('file', file)

  try {
    const { data } = await client.post<LintReport>('/api/v1/documents/lint', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
    return data
  } catch (err) {
    if (axios.isAxiosError(err) && err.response?.data) {
      const apiError = err.response.data as ApiError
      throw new ApiRequestError(apiError.error.code, apiError.error.message)
    }
    throw new ApiRequestError('NETWORK_ERROR', 'Failed to reach the server.')
  }
}

export async function healthCheck(): Promise<boolean> {
  try {
    await client.get('/healthz')
    return true
  } catch {
    return false
  }
}
