<template>
  <div class="rounded-lg border border-gray-200 bg-white p-8 text-center">
    <i class="pi pi-exclamation-circle mb-4 text-4xl text-yellow-500" />
    <h1 class="mb-2 text-2xl font-bold text-gray-800">
      404
    </h1>
    <p class="mb-4 text-sm text-gray-600">
      Page not found.
    </p>
    <RouterLink
      to="/lint"
      class="text-sm text-blue-600 underline hover:text-blue-800"
    >
      Go to Lint
    </RouterLink>
  </div>
</template>
