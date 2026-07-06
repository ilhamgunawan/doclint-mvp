<template>
  <div class="rounded-lg border-2 border-dashed border-gray-300 bg-white p-8 text-center">
    <div class="mb-4">
      <i class="pi pi-file-pdf text-4xl text-red-500" />
    </div>
    <h2 class="mb-2 text-lg font-semibold text-gray-700">
      Upload a PDF Document
    </h2>
    <p class="mb-4 text-sm text-gray-500">
      Select a PDF file to lint against formatting rules
    </p>
    <input
      ref="fileInput"
      type="file"
      accept="application/pdf"
      class="hidden"
      @change="onFileSelected"
    >
    <div class="flex justify-center gap-3">
      <Button
        label="Choose File"
        icon="pi pi-folder-open"
        outlined
        :disabled="disabled"
        @click="triggerFileInput"
      />
      <Button
        v-if="selectedFile"
        label="Run Lint"
        icon="pi pi-play"
        :loading="loading"
        :disabled="disabled"
        @click="$emit('lint', selectedFile)"
      />
    </div>
    <p
      v-if="selectedFile"
      class="mt-3 text-sm text-gray-600"
    >
      {{ selectedFile.name }} ({{ formatSize(selectedFile.size) }})
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import Button from 'primevue/button'

defineProps<{
  loading: boolean
  disabled: boolean
}>()

defineEmits<{
  lint: [file: File]
}>()

const fileInput = ref<HTMLInputElement | null>(null)
const selectedFile = ref<File | null>(null)

function triggerFileInput() {
  fileInput.value?.click()
}

function onFileSelected(event: Event) {
  const target = event.target as HTMLInputElement
  if (target.files && target.files.length > 0) {
    selectedFile.value = target.files[0]
  }
}

function formatSize(bytes: number): string {
  if (bytes < 1024) return bytes + ' B'
  if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB'
  return (bytes / (1024 * 1024)).toFixed(1) + ' MB'
}
</script>
