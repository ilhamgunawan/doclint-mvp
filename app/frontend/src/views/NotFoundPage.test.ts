import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import NotFoundPage from './NotFoundPage.vue'

const stubs = {
  RouterLink: {
    template: '<a :href="to"><slot /></a>',
    props: ['to'],
  },
}

describe('NotFoundPage', () => {
  it('renders the 404 heading', () => {
    render(NotFoundPage, { global: { stubs } })
    expect(screen.getByText('404')).toBeTruthy()
  })

  it('renders the page not found message', () => {
    render(NotFoundPage, { global: { stubs } })
    expect(screen.getByText('Page not found.')).toBeTruthy()
  })

  it('renders a link to the lint page', () => {
    render(NotFoundPage, { global: { stubs } })
    const link = screen.getByText('Go to Lint')
    expect(link).toBeTruthy()
    expect(link.getAttribute('href')).toBe('/lint')
  })
})
