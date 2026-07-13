import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/vue'
import UploadCard from './UploadCard.vue'

const stubs = {
  Button: {
    template: '<button :disabled="disabled" class="p-button">{{ label }}</button>',
    props: ['disabled', 'loading', 'icon', 'label', 'outlined'],
  },
}

function createFile(name: string, type: string, size?: number): File {
  return new File([new ArrayBuffer(size ?? 10)], name, { type })
}

describe('UploadCard', () => {
  it('renders the heading and description', () => {
    render(UploadCard, {
      props: { loading: false, disabled: false },
      global: { stubs },
    })
    expect(screen.getByText('Upload a PDF Document')).toBeTruthy()
    expect(screen.getByText(/Select a PDF file/)).toBeTruthy()
  })

  it('renders Choose File button', () => {
    render(UploadCard, {
      props: { loading: false, disabled: false },
      global: { stubs },
    })
    expect(screen.getByText('Choose File')).toBeTruthy()
  })

  it('does not show Run Lint button when no file is selected', () => {
    render(UploadCard, {
      props: { loading: false, disabled: false },
      global: { stubs },
    })
    expect(screen.queryByText('Run Lint')).toBeNull()
  })

  it('disables buttons when disabled prop is true', () => {
    render(UploadCard, {
      props: { loading: false, disabled: true },
      global: { stubs },
    })
    const buttons = screen.getAllByRole('button')
    buttons.forEach(btn => {
      expect((btn as HTMLButtonElement).disabled).toBe(true)
    })
  })

  it('shows file name and Run Lint button after file selection', async () => {
    const { container } = render(UploadCard, {
      props: { loading: false, disabled: false },
      global: { stubs },
    })

    const file = createFile('report.pdf', 'application/pdf')
    const input = container.querySelector('input[type="file"]') as HTMLInputElement

    Object.defineProperty(input, 'files', {
      value: [file],
      writable: false,
    })
    input.dispatchEvent(new Event('change', { bubbles: true }))

    await vi.waitFor(() => {
      expect(screen.getByText(/report.pdf/)).toBeTruthy()
    })
    expect(screen.getByText('Run Lint')).toBeTruthy()
  })

  it('emits lint event with the selected file', async () => {
    const { container, emitted } = render(UploadCard, {
      props: { loading: false, disabled: false },
      global: { stubs },
    })

    const file = createFile('report.pdf', 'application/pdf')
    const input = container.querySelector('input[type="file"]') as HTMLInputElement
    Object.defineProperty(input, 'files', {
      value: [file],
      writable: false,
    })
    input.dispatchEvent(new Event('change', { bubbles: true }))

    await vi.waitFor(() => {
      expect(screen.getByText('Run Lint')).toBeTruthy()
    })

    await screen.getByText('Run Lint').click()
    expect(emitted()).toHaveProperty('lint')
    expect(emitted().lint[0]).toEqual([file])
  })
})
