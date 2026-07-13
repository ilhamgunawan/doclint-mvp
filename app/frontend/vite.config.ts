import { defineConfig as defineViteConfig, mergeConfig } from 'vite'
import { defineConfig as defineVitestConfig } from 'vitest/config'
import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'

const viteConfig = defineViteConfig({
  plugins: [vue(), tailwindcss()],
})

const vitestConfig = defineVitestConfig({
  test: {
    // enable jest-like global test APIs
    globals: true,
    // simulate DOM with happy-dom
    // (requires installing happy-dom as a peer dependency)
    environment: 'happy-dom',
    coverage: {
      provider: 'v8',
      include: [
        'src/**/*.{ts,tsx}',
        'src/components/**/*.vue',
        'src/views/**/*.vue',
      ],
      exclude: [
        'src/vite-env.d.ts',
        'src/types/index.ts',
      ],
      thresholds: {
        lines: 90,
      },
    },
  }
})

export default mergeConfig(vitestConfig, viteConfig)
