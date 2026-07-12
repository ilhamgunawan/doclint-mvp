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
    <Accordion
      v-else
      :value="openPanels"
      :multiple="true"
      lazy
    >
      <AccordionPanel
        v-for="pageGroup in issuesByPage"
        :key="pageGroup.page"
        :value="pageGroup.page"
      >
        <AccordionHeader>
          <div class="flex w-full items-center justify-between pr-4">
            <span>Page {{ pageGroup.page }}</span>
            <Tag
              :value="`${pageGroup.issues.length} issue${pageGroup.issues.length !== 1 ? 's' : ''}`"
              severity="warn"
            />
          </div>
        </AccordionHeader>
        <AccordionContent>
          <div class="flex flex-col gap-2">
            <IssueCard
              v-for="(issue, index) in pageGroup.issues"
              :key="index"
              :issue="issue"
            />
          </div>
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
  </div>
</template>

<script setup lang="ts">
import Accordion from 'primevue/accordion'
import AccordionPanel from 'primevue/accordionpanel'
import AccordionHeader from 'primevue/accordionheader'
import AccordionContent from 'primevue/accordioncontent'
import Tag from 'primevue/tag'
import type { LintIssue, PageIssues } from '../types'
import IssueCard from './IssueCard.vue'

defineProps<{
  issues: LintIssue[]
  issuesByPage: PageIssues[]
}>()

const openPanels: number[] = []
</script>
