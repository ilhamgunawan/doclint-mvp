<template>
  <div class="rounded-lg border border-gray-200 bg-white p-6">
    <div class="mb-4 flex items-center justify-between">
      <h3 class="text-lg font-semibold text-gray-800">
        Document Summary
      </h3>
      <Tag
        :value="summary.status"
        :severity="summary.status === 'PASSED' ? 'success' : 'danger'"
      />
    </div>
    <div class="grid grid-cols-2 gap-4 text-sm">
      <div>
        <span class="text-gray-500">File:</span>
        <span class="ml-2 text-gray-800">{{ document.fileName }}</span>
      </div>
      <div>
        <span class="text-gray-500">Size:</span>
        <span class="ml-2 text-gray-800">{{ formatSize(document.fileSize) }}</span>
      </div>
      <div>
        <span class="text-gray-500">Pages:</span>
        <span class="ml-2 text-gray-800">{{ document.pageCount }}</span>
      </div>
      <div>
        <span class="text-gray-500">Page Size:</span>
        <span class="ml-2 text-gray-800">{{ document.pageSize }}</span>
      </div>
      <div>
        <span class="text-gray-500">Orientation:</span>
        <span class="ml-2 text-gray-800">{{ document.orientation }}</span>
      </div>
      <div>
        <span class="text-gray-500">Rules:</span>
        <span class="ml-2 text-gray-800">{{ summary.ruleCount }}</span>
      </div>
      <div>
        <span class="text-gray-500">Issues:</span>
        <span class="ml-2 text-gray-800">{{ summary.issueCount }}</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Document, Summary } from '../types'
import Tag from 'primevue/tag'

defineProps<{
  document: Document
  summary: Summary
}>()

function formatSize(bytes: number): string {
  if (bytes < 1024) return bytes + ' B'
  if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB'
  return (bytes / (1024 * 1024)).toFixed(1) + ' MB'
}
</script>
