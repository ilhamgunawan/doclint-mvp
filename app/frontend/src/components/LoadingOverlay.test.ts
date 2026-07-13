import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import LoadingOverlay from './LoadingOverlay.vue'

describe('LoadingOverlay', () => {
  it('is hidden when visible is false', () => {
    render(LoadingOverlay, { props: { visible: false } })
    expect(screen.queryByText(/Linting/)).toBeNull()
  })

  it('is visible when visible is true', () => {
    render(LoadingOverlay, { props: { visible: true, message: 'Linting document...' } })
    expect(screen.getByText('Linting document...')).toBeTruthy()
  })

  it('renders a spinner icon when visible', () => {
    render(LoadingOverlay, { props: { visible: true, message: 'Processing...' } })
    expect(screen.getByText('Processing...')).toBeTruthy()
  })
})
