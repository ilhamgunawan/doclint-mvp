import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import ErrorAlert from './ErrorAlert.vue'

describe('ErrorAlert', () => {
  it('is hidden when message is null', () => {
    render(ErrorAlert, { props: { message: null } })
    expect(screen.queryByText(/./)).toBeNull()
  })

  it('renders the error message', () => {
    render(ErrorAlert, { props: { message: 'Something went wrong' } })
    expect(screen.getByText('Something went wrong')).toBeTruthy()
  })

  it('emits dismiss when the close button is clicked', async () => {
    const { emitted } = render(ErrorAlert, {
      props: { message: 'An error occurred' },
    })
    await screen.getByRole('button').click()
    expect(emitted()).toHaveProperty('dismiss')
    expect(emitted().dismiss).toHaveLength(1)
  })
})
