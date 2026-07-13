import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import IssueList from './IssueList.vue'
import type { LintIssue, PageIssues } from '../types'

const stubs = {
  IssueCard: { template: '<div data-testid="issue-card" />' },
  Tag: { template: '<span class="p-tag">{{ value }}</span>', props: ['value', 'severity'] },
  Accordion: { template: '<div class="p-accordion"><slot /></div>' },
  AccordionPanel: { template: '<div class="p-accordion-panel"><slot /></div>' },
  AccordionHeader: { template: '<div class="p-accordion-header"><slot /></div>' },
  AccordionContent: { template: '<div class="p-accordion-content"><slot /></div>' },
}

const issuesByPage: PageIssues[] = [
  {
    page: 1,
    issues: [
      {
        rule: { id: 'r1', name: 'Font Embedding' },
        severity: 'Error', page: 1,
        expected: 'Embedded', actual: 'Not embedded',
        message: 'Font not embedded',
      },
    ],
  },
  {
    page: 3,
    issues: [
      {
        rule: { id: 'r2', name: 'Color Space' },
        severity: 'Warning', page: 3,
        expected: 'CMYK', actual: 'RGB',
        message: 'Wrong color space',
      },
      {
        rule: { id: 'r1', name: 'Font Embedding' },
        severity: 'Error', page: 3,
        expected: 'Embedded', actual: 'Not embedded',
        message: 'Font Y not embedded',
      },
    ],
  },
]

describe('IssueList', () => {
  it('renders the heading', () => {
    render(IssueList, {
      props: { issues: [], issuesByPage: [] },
      global: { stubs },
    })
    expect(screen.getByText(/Lint Issues/)).toBeTruthy()
  })

  it('shows "No issues found" when empty', () => {
    render(IssueList, {
      props: { issues: [], issuesByPage: [] },
      global: { stubs },
    })
    expect(screen.getByText('No issues found.')).toBeTruthy()
  })

  it('renders page headings for each page group', () => {
    render(IssueList, {
      props: { issues: issuesByPage.flatMap(p => p.issues), issuesByPage },
      global: { stubs },
    })
    expect(screen.getByText('Page 1')).toBeTruthy()
    expect(screen.getByText('Page 3')).toBeTruthy()
  })

  it('renders issue count per page', () => {
    render(IssueList, {
      props: { issues: issuesByPage.flatMap(p => p.issues), issuesByPage },
      global: { stubs },
    })
    expect(screen.getByText('1 issue')).toBeTruthy()
    expect(screen.getByText('2 issues')).toBeTruthy()
  })

  it('does not show "No issues found" when there are issues', () => {
    render(IssueList, {
      props: { issues: issuesByPage.flatMap(p => p.issues), issuesByPage },
      global: { stubs },
    })
    expect(screen.queryByText('No issues found.')).toBeNull()
  })

  it('renders heading with issue count in parentheses', () => {
    render(IssueList, {
      props: { issues: issuesByPage.flatMap(p => p.issues), issuesByPage },
      global: { stubs },
    })
    expect(screen.getByText('Lint Issues (3)')).toBeTruthy()
  })
})
