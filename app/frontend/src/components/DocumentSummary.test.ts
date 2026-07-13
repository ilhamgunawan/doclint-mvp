import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import DocumentSummary from './DocumentSummary.vue'
import type { Document, Summary } from '../types'

const stubs = {
  Tag: { template: '<span class="p-tag">{{ value }}</span>', props: ['value', 'severity'] },
}

const document: Document = {
  id: 'doc-1', fileName: 'report.pdf', fileSize: 204800,
  mimeType: 'application/pdf', pageCount: 5, pageSize: 'A4', orientation: 'portrait',
}

const summary: Summary = {
  status: 'Passed', ruleCount: 10, issueCount: 0,
}

describe('DocumentSummary', () => {
  it('renders the heading', () => {
    render(DocumentSummary, {
      props: { document, summary },
      global: { stubs },
    })
    expect(screen.getByText('Document Summary')).toBeTruthy()
  })

  it('renders document file name', () => {
    render(DocumentSummary, {
      props: { document, summary },
      global: { stubs },
    })
    expect(screen.getByText('report.pdf')).toBeTruthy()
  })

  it('renders formatted file size', () => {
    render(DocumentSummary, {
      props: { document, summary },
      global: { stubs },
    })
    expect(screen.getByText('200.0 KB')).toBeTruthy()
  })

  it('renders page count', () => {
    render(DocumentSummary, {
      props: { document, summary },
      global: { stubs },
    })
    expect(screen.getByText('5')).toBeTruthy()
  })

  it('renders page size', () => {
    render(DocumentSummary, {
      props: { document, summary },
      global: { stubs },
    })
    expect(screen.getByText('A4')).toBeTruthy()
  })

  it('renders orientation', () => {
    render(DocumentSummary, {
      props: { document, summary },
      global: { stubs },
    })
    expect(screen.getByText('portrait')).toBeTruthy()
  })

  it('renders summary status as Passed', () => {
    render(DocumentSummary, {
      props: { document, summary },
      global: { stubs },
    })
    expect(screen.getByText('Passed')).toBeTruthy()
  })

  it('renders summary issue count', () => {
    render(DocumentSummary, {
      props: { document, summary },
      global: { stubs },
    })
    expect(screen.getByText('0')).toBeTruthy()
  })

  it('formats bytes less than 1024 as B', () => {
    const smallDoc: Document = { ...document, fileSize: 500 }
    render(DocumentSummary, {
      props: { document: smallDoc, summary },
      global: { stubs },
    })
    expect(screen.getByText('500 B')).toBeTruthy()
  })

  it('formats bytes >= 1 MB as MB', () => {
    const largeDoc: Document = { ...document, fileSize: 3145728 }
    render(DocumentSummary, {
      props: { document: largeDoc, summary },
      global: { stubs },
    })
    expect(screen.getByText('3.0 MB')).toBeTruthy()
  })
})
