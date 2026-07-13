/* eslint-disable @typescript-eslint/no-explicit-any */
import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen } from '@testing-library/vue'
import LintPage from './LintPage.vue'

const { mockStore } = vi.hoisted(() => ({
  mockStore: {
    isLinting: false,
    isUploading: false,
    error: null,
    document: null,
    summary: null,
    ruleResults: [] as Array<Record<string, unknown>>,
    issues: [] as Array<Record<string, unknown>>,
    issuesByPage: [] as Array<Record<string, unknown>>,
    lint: vi.fn(),
  },
}))

vi.mock('../stores/lintStore', () => ({
  useLintStore: () => mockStore,
}))

const stubs = {
  UploadCard: {
    template: '<div data-testid="upload-card"><button data-testid="lint-btn" @click="$emit(\'lint\', { name: \'test.pdf\' })">Lint</button></div>',
    emits: ['lint'],
  },
  ErrorAlert: {
    template: '<div data-testid="error-alert" />',
  },
  DocumentSummary: {
    template: '<div data-testid="document-summary" />',
  },
  RuleResultList: {
    template: '<div data-testid="rule-result-list" />',
  },
  IssueList: {
    template: '<div data-testid="issue-list" />',
  },
  LoadingOverlay: {
    template: '<div data-testid="loading-overlay" />',
  },
}

describe('LintPage', () => {
  beforeEach(() => {
    mockStore.isLinting = false
    mockStore.isUploading = false
    mockStore.error = null
    mockStore.document = null
    mockStore.summary = null
    mockStore.ruleResults = []
    mockStore.issues = []
    mockStore.issuesByPage = []
    vi.clearAllMocks()
  })

  it('renders UploadCard always', () => {
    render(LintPage, { global: { stubs } })
    expect(screen.getByTestId('upload-card')).toBeTruthy()
  })

  it('renders ErrorAlert always', () => {
    render(LintPage, { global: { stubs } })
    expect(screen.getByTestId('error-alert')).toBeTruthy()
  })

  it('renders LoadingOverlay always', () => {
    render(LintPage, { global: { stubs } })
    expect(screen.getByTestId('loading-overlay')).toBeTruthy()
  })

  it('does not render document results when no document', () => {
    render(LintPage, { global: { stubs } })
    expect(screen.queryByTestId('document-summary')).toBeNull()
    expect(screen.queryByTestId('rule-result-list')).toBeNull()
    expect(screen.queryByTestId('issue-list')).toBeNull()
  })

  it('renders document results when document and summary present', () => {
    ;(mockStore.document as any) = {
      id: 'doc-1', fileName: 'test.pdf', fileSize: 100,
      mimeType: 'application/pdf', pageCount: 1, pageSize: 'A4', orientation: 'portrait',
    }
    ;(mockStore.summary as any) = { status: 'Passed', ruleCount: 0, issueCount: 0 }

    render(LintPage, { global: { stubs } })

    expect(screen.getByTestId('document-summary')).toBeTruthy()
    expect(screen.getByTestId('rule-result-list')).toBeTruthy()
    expect(screen.getByTestId('issue-list')).toBeTruthy()
  })

  it('calls store.lint when UploadCard emits lint event', async () => {
    render(LintPage, { global: { stubs } })
    screen.getByTestId('lint-btn').click()
    expect(mockStore.lint).toHaveBeenCalledOnce()
    expect(mockStore.lint).toHaveBeenCalledWith({ name: 'test.pdf' })
  })
})
