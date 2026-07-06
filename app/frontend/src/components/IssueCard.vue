<template>
  <div class="rounded-md border-l-4 border-red-400 bg-red-50 px-4 py-3">
    <div class="mb-1 flex items-center justify-between">
      <span class="text-sm font-semibold text-gray-800">
        Page {{ issue.page }} &mdash; {{ issue.rule.name }}
      </span>
      <Tag
        :value="issue.severity"
        severity="warn"
      />
    </div>
    <p class="text-sm text-gray-600">
      {{ issue.message }}
    </p>
    <div class="mt-1 text-xs text-gray-500">
      Expected: <span class="font-mono">{{ issue.expected }}</span>
      &middot;
      Actual: <span class="font-mono">{{ issue.actual }}</span>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { LintIssue } from '../types'
import Tag from 'primevue/tag'

defineProps<{
  issue: LintIssue
}>()
</script>
