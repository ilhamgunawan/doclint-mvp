import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import IssueCard from './IssueCard.vue'
import type { LintIssue } from '../types'

const stubs = {
  Tag: { template: '<span class="p-tag">{{ value }}</span>', props: ['value', 'severity'] },
}

const errorIssue: LintIssue = {
  rule: { id: 'r1', name: 'Font Embedding' },
  severity: 'Error',
  page: 3,
  expected: 'All fonts should be embedded',
  actual: 'Font X is not embedded',
  message: 'Font X on page 3 is not embedded',
}

const warningIssue: LintIssue = {
  rule: { id: 'r2', name: 'Color Space' },
  severity: 'Warning',
  page: 1,
  expected: 'CMYK color space',
  actual: 'RGB color space used',
  message: 'Page 1 uses RGB instead of CMYK',
}

describe('IssueCard', () => {
  it('renders the rule name', () => {
    render(IssueCard, { props: { issue: errorIssue }, global: { stubs } })
    expect(screen.getByText('Font Embedding')).toBeTruthy()
  })

  it('renders the severity', () => {
    render(IssueCard, { props: { issue: errorIssue }, global: { stubs } })
    expect(screen.getByText('Error')).toBeTruthy()
  })

  it('renders the message', () => {
    render(IssueCard, { props: { issue: errorIssue }, global: { stubs } })
    expect(screen.getByText('Font X on page 3 is not embedded')).toBeTruthy()
  })

  it('renders expected value', () => {
    render(IssueCard, { props: { issue: errorIssue }, global: { stubs } })
    expect(screen.getByText('All fonts should be embedded')).toBeTruthy()
  })

  it('renders actual value', () => {
    render(IssueCard, { props: { issue: errorIssue }, global: { stubs } })
    expect(screen.getByText('Font X is not embedded')).toBeTruthy()
  })

  it('applies error border class for Error severity', () => {
    const { container } = render(IssueCard, { props: { issue: errorIssue }, global: { stubs } })
    expect(container.querySelector('.border-red-400')).toBeTruthy()
  })

  it('applies warning border class for Warning severity', () => {
    const { container } = render(IssueCard, { props: { issue: warningIssue }, global: { stubs } })
    expect(container.querySelector('.border-yellow-400')).toBeTruthy()
  })
})
