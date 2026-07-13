import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/vue'
import AboutPage from './AboutPage.vue'

describe('AboutPage', () => {
  it('renders the heading', () => {
    render(AboutPage)
    expect(screen.getByText('About DocLint')).toBeTruthy()
  })

  it('renders the description', () => {
    render(AboutPage)
    expect(screen.getByText(/DocLint is a PDF document format validator/)).toBeTruthy()
  })
})
