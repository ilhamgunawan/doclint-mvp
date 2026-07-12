import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Document, Summary, RuleResult, LintIssue, PageIssues } from '../types'
import { lintDocument, ApiRequestError } from '../services/documentService'

export const useLintStore = defineStore('lint', () => {
  const document = ref<Document | null>(null)
  const summary = ref<Summary | null>(null)
  const ruleResults = ref<RuleResult[]>([])
  const issues = ref<LintIssue[]>([])
  const isUploading = ref(false)
  const isLinting = ref(false)
  const error = ref<string | null>(null)

  const issuesByPage = computed<PageIssues[]>(() => {
    const map = new Map<number, LintIssue[]>()
    for (const issue of issues.value) {
      if (!map.has(issue.page)) {
        map.set(issue.page, [])
      }
      map.get(issue.page)!.push(issue)
    }
    return Array.from(map.entries())
      .map(([page, pageIssues]) => ({ page, issues: pageIssues }))
      .sort((a, b) => a.page - b.page)
  })

  async function lint(file: File) {
    error.value = null
    isUploading.value = true

    const allowedTypes = ['application/pdf']
    if (!allowedTypes.includes(file.type)) {
      error.value = 'Only PDF files are supported.'
      isUploading.value = false
      return
    }

    const maxSize = 50 * 1024 * 1024
    if (file.size > maxSize) {
      error.value = 'File exceeds the 50 MB size limit.'
      isUploading.value = false
      return
    }

    isLinting.value = true

    try {
      const report = await lintDocument(file)
      document.value = report.document
      summary.value = report.summary
      ruleResults.value = report.ruleResults
      issues.value = report.issues
    } catch (err) {
      if (err instanceof ApiRequestError) {
        error.value = err.message
      } else {
        error.value = 'An unexpected error occurred.'
      }
    } finally {
      isUploading.value = false
      isLinting.value = false
    }
  }

  function reset() {
    document.value = null
    summary.value = null
    ruleResults.value = []
    issues.value = []
    isUploading.value = false
    isLinting.value = false
    error.value = null
  }

  return {
    document,
    summary,
    ruleResults,
    issues,
    issuesByPage,
    isUploading,
    isLinting,
    error,
    lint,
    reset,
  }
})
