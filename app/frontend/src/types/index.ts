export interface Document {
  id: string
  fileName: string
  fileSize: number
  mimeType: string
  pageCount: number
  pageSize: string
  orientation: string
}

export interface LintRule {
  id: string
  name: string
}

export interface LintIssue {
  rule: LintRule
  severity: 'Error' | 'Warning'
  page: number
  expected: string
  actual: string
  message: string
}

export interface Summary {
  status: 'Passed' | 'Failed'
  ruleCount: number
  issueCount: number
}

export interface RuleResult {
  rule: string
  status: 'Passed' | 'Failed'
  issueCount: number
}

export interface LintReport {
  document: Document
  summary: Summary
  ruleResults: RuleResult[]
  issues: LintIssue[]
}

export interface ApiError {
  error: {
    code: string
    message: string
  }
}
