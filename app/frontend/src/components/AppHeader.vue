<template>
  <header class="border-b border-gray-200 bg-white shadow-sm">
    <div class="mx-auto flex max-w-4xl items-center justify-between px-4 py-3">
      <RouterLink
        to="/lint"
        class="text-xl font-bold text-gray-800 no-underline"
      >
        DocLint
      </RouterLink>
      <nav class="flex gap-4">
        <RouterLink
          to="/lint"
          class="text-sm text-gray-600 hover:text-gray-900"
        >
          Lint
        </RouterLink>
        <RouterLink
          to="/about"
          class="text-sm text-gray-600 hover:text-gray-900"
        >
          About
        </RouterLink>
      </nav>
    </div>
  </header>
</template>
