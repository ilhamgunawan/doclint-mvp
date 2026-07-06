<template>
  <div class="space-y-6">
    <UploadCard
      :loading="store.isLinting"
      :disabled="store.isUploading || store.isLinting"
      @lint="handleLint"
    />

    <ErrorAlert
      :message="store.error"
      @dismiss="store.error = null"
    />

    <template v-if="store.document && store.summary">
      <DocumentSummary
        :document="store.document"
        :summary="store.summary"
      />

      <RuleResultList :results="store.ruleResults" />

      <IssueList :issues="store.issues" />
    </template>

    <LoadingOverlay
      :visible="store.isLinting"
      message="Linting document..."
    />
  </div>
</template>

<script setup lang="ts">
import { useLintStore } from '../stores/lintStore'
import UploadCard from '../components/UploadCard.vue'
import ErrorAlert from '../components/ErrorAlert.vue'
import DocumentSummary from '../components/DocumentSummary.vue'
import RuleResultList from '../components/RuleResultList.vue'
import IssueList from '../components/IssueList.vue'
import LoadingOverlay from '../components/LoadingOverlay.vue'

const store = useLintStore()

async function handleLint(file: File) {
  await store.lint(file)
}
</script>
