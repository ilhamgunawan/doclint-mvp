import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useLintStore } from './lintStore'
import type { LintIssue, LintReport } from '../types'

const { mockLintDocument, MockApiRequestError } = vi.hoisted(() => ({
  mockLintDocument: vi.fn(),
  MockApiRequestError: class extends Error {
    code: string
    constructor(code: string, message: string) {
      super(message)
      this.code = code
    }
  },
}))

vi.mock('../services/documentService', () => ({
  lintDocument: mockLintDocument,
  ApiRequestError: MockApiRequestError,
}))

function createFile(name: string, type: string, size?: number): File {
  return new File([new ArrayBuffer(size ?? 10)], name, { type })
}

const mockReport: LintReport = {
  document: {
    id: 'doc-1',
    fileName: 'report.pdf',
    fileSize: 1024,
    mimeType: 'application/pdf',
    pageCount: 3,
    pageSize: 'A4',
    orientation: 'portrait',
  },
  summary: {
    status: 'Passed',
    ruleCount: 5,
    issueCount: 0,
  },
  ruleResults: [
    { rule: 'PDF/A Compliance', status: 'Passed', issueCount: 0 },
    { rule: 'Font Validation', status: 'Passed', issueCount: 0 },
  ],
  issues: [],
}

const mockIssues: LintIssue[] = [
  {
    rule: { id: 'r1', name: 'Font Embedding' },
    severity: 'Error',
    page: 3,
    expected: 'All fonts should be embedded',
    actual: 'Font X is not embedded',
    message: 'Font X on page 3 is not embedded',
  },
  {
    rule: { id: 'r2', name: 'Color Space' },
    severity: 'Warning',
    page: 1,
    expected: 'CMYK color space',
    actual: 'RGB color space used',
    message: 'Page 1 uses RGB instead of CMYK',
  },
  {
    rule: { id: 'r1', name: 'Font Embedding' },
    severity: 'Error',
    page: 1,
    expected: 'All fonts should be embedded',
    actual: 'Font Y is not embedded',
    message: 'Font Y on page 1 is not embedded',
  },
]

describe('lintStore', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
  })

  describe('initial state', () => {
    it('initializes with default values', () => {
      const store = useLintStore()
      expect(store.document).toBeNull()
      expect(store.summary).toBeNull()
      expect(store.ruleResults).toEqual([])
      expect(store.issues).toEqual([])
      expect(store.isUploading).toBe(false)
      expect(store.isLinting).toBe(false)
      expect(store.error).toBeNull()
    })
  })

  describe('issuesByPage', () => {
    it('groups issues by page and sorts ascending', () => {
      const store = useLintStore()
      store.issues = mockIssues

      expect(store.issuesByPage).toHaveLength(2)
      expect(store.issuesByPage[0]).toEqual({
        page: 1,
        issues: [mockIssues[1], mockIssues[2]],
      })
      expect(store.issuesByPage[1]).toEqual({
        page: 3,
        issues: [mockIssues[0]],
      })
    })

    it('returns empty array when there are no issues', () => {
      const store = useLintStore()
      expect(store.issuesByPage).toEqual([])
    })
  })

  describe('lint', () => {
    it('rejects non-PDF files', async () => {
      const store = useLintStore()
      const file = createFile('doc.txt', 'text/plain')
      await store.lint(file)
      expect(store.error).toBe('Only PDF files are supported.')
      expect(mockLintDocument).not.toHaveBeenCalled()
    })

    it('rejects files exceeding 50 MB', async () => {
      const store = useLintStore()
      const file = createFile('big.pdf', 'application/pdf', 51 * 1024 * 1024 + 1)
      await store.lint(file)
      expect(store.error).toBe('File exceeds the 50 MB size limit.')
      expect(mockLintDocument).not.toHaveBeenCalled()
    })

    it('sets isUploading to true before processing', async () => {
      const store = useLintStore()
      mockLintDocument.mockResolvedValue(mockReport)
      const file = createFile('report.pdf', 'application/pdf')
      const promise = store.lint(file)
      expect(store.isUploading).toBe(true)
      await promise
    })

    it('sets document, summary, ruleResults, and issues on success', async () => {
      const store = useLintStore()
      mockLintDocument.mockResolvedValue(mockReport)
      const file = createFile('report.pdf', 'application/pdf')
      await store.lint(file)
      expect(store.document).toEqual(mockReport.document)
      expect(store.summary).toEqual(mockReport.summary)
      expect(store.ruleResults).toEqual(mockReport.ruleResults)
      expect(store.issues).toEqual(mockReport.issues)
    })

    it('clears error on successful lint', async () => {
      const store = useLintStore()
      store.error = 'Previous error'
      mockLintDocument.mockResolvedValue(mockReport)
      const file = createFile('report.pdf', 'application/pdf')
      await store.lint(file)
      expect(store.error).toBeNull()
    })

    it('sets error to API message on ApiRequestError', async () => {
      const store = useLintStore()
      mockLintDocument.mockRejectedValue(
        new MockApiRequestError('PDF_MALFORMED', 'The PDF is corrupted')
      )
      const file = createFile('report.pdf', 'application/pdf')
      await store.lint(file)
      expect(store.error).toBe('The PDF is corrupted')
    })

    it('sets generic error on unexpected exception', async () => {
      const store = useLintStore()
      mockLintDocument.mockRejectedValue(new Error('Something went wrong'))
      const file = createFile('report.pdf', 'application/pdf')
      await store.lint(file)
      expect(store.error).toBe('An unexpected error occurred.')
    })

    it('resets isUploading and isLinting after success', async () => {
      const store = useLintStore()
      mockLintDocument.mockResolvedValue(mockReport)
      const file = createFile('report.pdf', 'application/pdf')
      await store.lint(file)
      expect(store.isUploading).toBe(false)
      expect(store.isLinting).toBe(false)
    })

    it('resets isUploading and isLinting even on error', async () => {
      const store = useLintStore()
      mockLintDocument.mockRejectedValue(new MockApiRequestError('ERR', 'fail'))
      const file = createFile('report.pdf', 'application/pdf')
      await store.lint(file)
      expect(store.isUploading).toBe(false)
      expect(store.isLinting).toBe(false)
    })

    it('does not call lintDocument when validation fails', async () => {
      const store = useLintStore()
      const txtFile = createFile('doc.txt', 'text/plain')
      await store.lint(txtFile)
      expect(mockLintDocument).not.toHaveBeenCalled()

      const bigFile = createFile('big.pdf', 'application/pdf', 51 * 1024 * 1024 + 1)
      await store.lint(bigFile)
      expect(mockLintDocument).not.toHaveBeenCalled()
    })
  })

  describe('reset', () => {
    it('clears all state to initial values', () => {
      const store = useLintStore()
      store.document = mockReport.document
      store.summary = mockReport.summary
      store.ruleResults = mockReport.ruleResults
      store.issues = mockIssues
      store.isUploading = true
      store.isLinting = true
      store.error = 'Some error'

      store.reset()

      expect(store.document).toBeNull()
      expect(store.summary).toBeNull()
      expect(store.ruleResults).toEqual([])
      expect(store.issues).toEqual([])
      expect(store.isUploading).toBe(false)
      expect(store.isLinting).toBe(false)
      expect(store.error).toBeNull()
    })
  })
})
