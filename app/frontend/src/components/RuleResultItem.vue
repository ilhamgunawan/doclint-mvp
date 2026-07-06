<template>
  <div class="flex items-center justify-between rounded-md border px-4 py-3">
    <div class="flex items-center gap-3">
      <i
        :class="iconClass"
        class="text-lg"
      />
      <span class="text-sm font-medium text-gray-700">{{ result.rule }}</span>
    </div>
    <div class="flex items-center gap-3">
      <span class="text-xs text-gray-400">{{ result.issueCount }} issue{{ result.issueCount !== 1 ? 's' : '' }}</span>
      <Tag
        :value="result.status"
        :severity="result.status === 'PASSED' ? 'success' : 'danger'"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import type { RuleResult } from '../types'
import Tag from 'primevue/tag'

const props = defineProps<{
  result: RuleResult
}>()

const iconClass = computed(() =>
  props.result.status === 'PASSED'
    ? 'pi pi-check-circle text-green-500'
    : 'pi pi-times-circle text-red-500',
)
</script>
