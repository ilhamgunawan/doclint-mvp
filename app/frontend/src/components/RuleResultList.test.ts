import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import RuleResultList from './RuleResultList.vue'
import type { RuleResult } from '../types'

const stubs = { RuleResultItem: true }

const results: RuleResult[] = [
  { rule: 'PDF/A Compliance', status: 'Passed', issueCount: 0 },
  { rule: 'Font Validation', status: 'Failed', issueCount: 3 },
  { rule: 'Color Space', status: 'Passed', issueCount: 0 },
]

describe('RuleResultList', () => {
  it('renders the heading', () => {
    render(RuleResultList, { props: { results }, global: { stubs } })
    expect(screen.getByText('Rule Results')).toBeTruthy()
  })

  it('renders one RuleResultItem per result', () => {
    const { container } = render(RuleResultList, { props: { results }, global: { stubs } })
    const items = container.querySelectorAll('rule-result-item-stub')
    expect(items.length).toBe(results.length)
  })

  it('renders nothing when results is empty', () => {
    const { container } = render(RuleResultList, { props: { results: [] }, global: { stubs } })
    const items = container.querySelectorAll('rule-result-item-stub')
    expect(items.length).toBe(0)
  })
})
