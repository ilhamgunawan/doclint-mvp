import { describe, it, expect, vi, beforeEach } from 'vitest'
import { lintDocument, healthCheck } from './documentService'

const { mockAxiosInstance, mockIsAxiosError } = vi.hoisted(() => {
  const instance = {
    post: vi.fn(),
    get: vi.fn(),
  }
  return {
    mockAxiosInstance: instance,
    mockIsAxiosError: vi.fn(),
  }
})

vi.mock('axios', () => ({
  default: {
    create: vi.fn(() => mockAxiosInstance),
    isAxiosError: mockIsAxiosError,
  },
}))

function createFile(name: string, type: string, size?: number): File {
  return new File([new ArrayBuffer(size ?? 10)], name, { type })
}

describe('documentService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('lintDocument', () => {
    const mockReport = {
      document: { id: 'doc-1', fileName: 'test.pdf', fileSize: 100, mimeType: 'application/pdf', pageCount: 1, pageSize: 'A4', orientation: 'portrait' },
      summary: { status: 'Passed', ruleCount: 0, issueCount: 0 },
      ruleResults: [],
      issues: [],
    }

    it('returns LintReport on successful POST', async () => {
      mockAxiosInstance.post.mockResolvedValue({ data: mockReport })

      const file = createFile('report.pdf', 'application/pdf')
      const result = await lintDocument(file)

      expect(mockAxiosInstance.post).toHaveBeenCalledWith(
        '/api/v1/documents/lint',
        expect.any(FormData),
        { headers: { 'Content-Type': 'multipart/form-data' } },
      )
      expect(result).toEqual(mockReport)
    })

    it('throws ApiRequestError with API error code and message on axios error with response data', async () => {
      const apiError = { error: { code: 'PDF_MALFORMED', message: 'The PDF is corrupted' } }
      const axiosError = new Error('Request failed')
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      ;(axiosError as any).response = { data: apiError }
      mockAxiosInstance.post.mockRejectedValue(axiosError)
      mockIsAxiosError.mockReturnValue(true)

      const file = createFile('report.pdf', 'application/pdf')
      await expect(lintDocument(file)).rejects.toMatchObject({
        code: 'PDF_MALFORMED',
        message: 'The PDF is corrupted',
      })
    })

    it('throws ApiRequestError with NETWORK_ERROR on axios error without response data', async () => {
      const axiosError = new Error('Network error')
      mockAxiosInstance.post.mockRejectedValue(axiosError)
      mockIsAxiosError.mockReturnValue(true)

      const file = createFile('report.pdf', 'application/pdf')
      await expect(lintDocument(file)).rejects.toMatchObject({
        code: 'NETWORK_ERROR',
        message: 'Failed to reach the server.',
      })
    })

    it('throws ApiRequestError with NETWORK_ERROR on non-axios error', async () => {
      const error = new Error('Something went wrong')
      mockAxiosInstance.post.mockRejectedValue(error)
      mockIsAxiosError.mockReturnValue(false)

      const file = createFile('report.pdf', 'application/pdf')
      await expect(lintDocument(file)).rejects.toMatchObject({
        code: 'NETWORK_ERROR',
        message: 'Failed to reach the server.',
      })
    })
  })

  describe('healthCheck', () => {
    it('returns true when GET /healthz succeeds', async () => {
      mockAxiosInstance.get.mockResolvedValue({})

      const result = await healthCheck()

      expect(mockAxiosInstance.get).toHaveBeenCalledWith('/healthz')
      expect(result).toBe(true)
    })

    it('returns false when GET /healthz fails', async () => {
      mockAxiosInstance.get.mockRejectedValue(new Error('Server down'))

      const result = await healthCheck()

      expect(result).toBe(false)
    })
  })
})
