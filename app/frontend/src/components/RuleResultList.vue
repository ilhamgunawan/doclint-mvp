<template>
  <div class="rounded-lg border border-gray-200 bg-white p-6">
    <h3 class="mb-4 text-lg font-semibold text-gray-800">
      Rule Results
    </h3>
    <div class="space-y-2">
      <RuleResultItem
        v-for="result in results"
        :key="result.rule"
        :result="result"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { RuleResult } from '../types'
import RuleResultItem from './RuleResultItem.vue'

defineProps<{
  results: RuleResult[]
}>()
</script>
