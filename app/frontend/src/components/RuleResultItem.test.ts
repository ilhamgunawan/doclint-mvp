import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import RuleResultItem from './RuleResultItem.vue'
import type { RuleResult } from '../types'

const stubs = {
  Tag: { template: '<span class="p-tag">{{ value }}</span>', props: ['value', 'severity'] },
}

const passedResult: RuleResult = { rule: 'PDF/A Compliance', status: 'Passed', issueCount: 0 }
const failedResult: RuleResult = { rule: 'Font Validation', status: 'Failed', issueCount: 3 }

describe('RuleResultItem', () => {
  it('renders the rule name', () => {
    render(RuleResultItem, { props: { result: passedResult }, global: { stubs } })
    expect(screen.getByText('PDF/A Compliance')).toBeTruthy()
  })

  it('renders the status', () => {
    render(RuleResultItem, { props: { result: passedResult }, global: { stubs } })
    expect(screen.getByText('Passed')).toBeTruthy()
  })

  it('renders the issue count', () => {
    render(RuleResultItem, { props: { result: failedResult }, global: { stubs } })
    expect(screen.getByText('3 issues')).toBeTruthy()
  })

  it('uses singular for one issue', () => {
    const singleIssueResult: RuleResult = { rule: 'Margins', status: 'Failed', issueCount: 1 }
    render(RuleResultItem, { props: { result: singleIssueResult }, global: { stubs } })
    expect(screen.getByText('1 issue')).toBeTruthy()
  })

  it('renders passed icon for passed result', () => {
    const { container } = render(RuleResultItem, {
      props: { result: passedResult },
      global: { stubs },
    })
    const icon = container.querySelector('.pi-check-circle')
    expect(icon).toBeTruthy()
  })

  it('renders failed icon for failed result', () => {
    const { container } = render(RuleResultItem, {
      props: { result: failedResult },
      global: { stubs },
    })
    const icon = container.querySelector('.pi-times-circle')
    expect(icon).toBeTruthy()
  })
})
