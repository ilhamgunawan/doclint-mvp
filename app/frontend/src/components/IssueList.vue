<template>
  <div class="rounded-lg border border-gray-200 bg-white p-6">
    <h3 class="mb-4 text-lg font-semibold text-gray-800">
      Lint Issues ({{ issues.length }})
    </h3>
    <div
      v-if="issues.length === 0"
      class="py-4 text-center text-sm text-gray-500"
    >
      No issues found.
    </div>
    <div
      v-else
      class="space-y-3"
    >
      <IssueCard
        v-for="(issue, index) in issues"
        :key="index"
        :issue="issue"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { LintIssue } from '../types'
import IssueCard from './IssueCard.vue'

defineProps<{
  issues: LintIssue[]
}>()
</script>
