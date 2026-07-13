import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import AppHeader from './AppHeader.vue'

const stubs = {
  RouterLink: {
    template: '<a :href="to"><slot /></a>',
    props: ['to'],
  },
}

describe('AppHeader', () => {
  it('renders the app title', () => {
    render(AppHeader, { global: { stubs } })
    expect(screen.getByText('DocLint')).toBeTruthy()
  })

  it('renders Lint navigation link pointing to /lint', () => {
    render(AppHeader, { global: { stubs } })
    const link = screen.getByText('Lint')
    expect(link).toBeTruthy()
    expect(link.getAttribute('href')).toBe('/lint')
  })

  it('renders About navigation link pointing to /about', () => {
    render(AppHeader, { global: { stubs } })
    const link = screen.getByText('About')
    expect(link).toBeTruthy()
    expect(link.getAttribute('href')).toBe('/about')
  })
})
