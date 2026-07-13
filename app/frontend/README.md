# DocLint Frontend

Vue 3 + TypeScript frontend for the DocLint PDF document format validator.

## Tech Stack

- **Vue 3** + **TypeScript** + **Vite**
- **Vue Router**
- **Pinia**
- **PrimeVue** + **Tailwind CSS**
- **Axios**

## Prerequisites

- Node.js 20+

## Setup

```bash
npm install
```

## Run

```bash
npm run dev
```

Opens at `http://localhost:5173`.

## Build

```bash
npm run build
```

Produces the production bundle in `dist/`.

## Test

Runs unit tests with [Vitest](https://vitest.dev/) in single-run mode:

```bash
npm run test
```

Run unit tests with coverage:

```bash
npm run test:ci
```

### Adding New Test

Place test files alongside the source file they test using a `.test.ts` suffix. For example, `src/services/documentService.ts` → `src/services/documentService.test.ts`.

```ts
import { describe, it, expect, vi, beforeEach } from 'vitest'

// Hoist mock factories before vi.mock calls
const { mockFn } = vi.hoisted(() => ({
  mockFn: vi.fn(),
}))

vi.mock('../path/to/module', () => ({
  exportName: mockFn,
}))

beforeEach(() => {
  vi.clearAllMocks()
})

describe('featureName', () => {
  it('works correctly', () => {
    mockFn.mockReturnValue(42)
    const result = someFunction()
    expect(result).toBe(42)
  })
})
```
