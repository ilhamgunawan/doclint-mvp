# API Specifications

## Overview

This document defines the REST API for the DocLint MVP.

The MVP API is intentionally simple. It provides a synchronous linting workflow where the client uploads a PDF document and immediately receives a lint report.

Future versions may introduce asynchronous processing, persistent storage, authentication, and document history.

---

# Base URL

```text
/api/v1
```

---

# Design Principles

## RESTful

The API follows REST principles and is resource-oriented.

Example:

```text
POST /api/v1/documents/lint
```

---

## Synchronous Processing

For the MVP, document linting is performed synchronously.

Workflow:

```text
Client -> Upload PDF -> API -> Lint Engine -> Lint Report -> Client
```

---

# Authentication

Authentication is not required for the MVP.

---

# Endpoints

## Health Check

### Request

```http
GET /healthz
```

### Response

```text
Healthy
```

### Status Codes

| Code | Description    |
| ---- | -------------- |
| 200  | API is healthy |

---

## Lint Document

Uploads a PDF document and executes all predefined lint rules.

### Request

```http
POST /api/v1/documents/lint
Content-Type: multipart/form-data
```

### Request Body

| Field | Type | Required | Description               |
| ----- | ---- | -------- | ------------------------- |
| file  | PDF  | Yes      | PDF document to be linted |

---

### Successful Response

```json
{
  "document": {
    "id": "8f8b8b84-3d5d-4c2b-b26d-64c5f2cb4fd0",
    "fileName": "thesis.pdf",
    "fileSize": 2457638,
    "mimeType": "application/pdf",
    "pageCount": 32,
    "pageSize": "A4",
    "orientation": "Portrait"
  },
  "summary": {
    "status": "FAILED",
    "ruleCount": 5,
    "issueCount": 17
  },
  "ruleResults": [
    {
      "rule": "font-size",
      "status": "FAILED",
      "issueCount": 1
    },
    {
      "rule": "font-family",
      "status": "FAILED",
      "issueCount": 1
    },
    {
      "rule": "margin",
      "status": "PASSED",
      "issueCount": 0
    },
    {
      "rule": "page-size",
      "status": "FAILED",
      "issueCount": 1
    },
    {
      "rule": "page-orientation",
      "status": "PASSED",
      "issueCount": 0
    }
  ],
  "issues": [
    {
      "rule": {
        "id": "font-size",
        "name": "Font Size"
      },
      "severity": "Error",
      "page": 4,
      "expected": "12 pt",
      "actual": "11 pt",
      "message": "Incorrect font size."
    },
    {
      "rule": {
        "id": "font-family",
        "name": "Font Family"
      },
      "severity": "Error",
      "page": 4,
      "expected": "Arial",
      "actual": "Times New Roman",
      "message": "Incorrect font family."
    },
    {
      "rule": {
        "id": "page-size",
        "name": "Page Size"
      },
      "severity": "Error",
      "page": 1,
      "expected": "A4",
      "actual": "Letter",
      "message": "Document uses an unsupported page size."
    }
  ]
}
```

---

### Response Fields

#### Document

Information about the uploaded document.

| Field       | Type          | Description                                                                       |
| ----------- | ------------- | --------------------------------------------------------------------------------- |
| id          | string (UUID) | Unique identifier for this lint request.                                          |
| fileName    | string        | Original uploaded file name.                                                      |
| fileSize    | integer       | File size in bytes.                                                               |
| mimeType    | string        | MIME type of the uploaded file.                                                   |
| pageCount   | integer       | Total number of pages in the document.                                            |
| pageSize    | string        | Detected page size. Returns `Mixed` if multiple page sizes are detected.          |
| orientation | string        | Detected page orientation. Returns `Mixed` if multiple orientations are detected. |

---

#### Summary

Overall result of the linting process.

| Field      | Type    | Description                                               |
| ---------- | ------- | --------------------------------------------------------- |
| status     | string  | Overall lint result. Possible values: `PASSED`, `FAILED`. |
| ruleCount  | integer | Total number of executed lint rules.                      |
| issueCount | integer | Total number of lint issues found across all rules.       |

---

#### Rule Results

Summary of the execution result for each lint rule.

| Field      | Type    | Description                                              |
| ---------- | ------- | -------------------------------------------------------- |
| rule       | string  | Unique identifier of the lint rule.                      |
| status     | string  | Result of the rule. Possible values: `PASSED`, `FAILED`. |
| issueCount | integer | Number of issues generated by this rule.                 |

---

#### Issues

Detailed information about every detected lint issue.

| Field     | Type    | Description                                          |
| --------- | ------- | ---------------------------------------------------- |
| rule.id   | string  | Unique identifier of the lint rule.                  |
| rule.name | string  | Human-readable lint rule name.                       |
| severity  | string  | Severity level. Possible values: `Error`, `Warning`. |
| page      | integer | Physical page where the issue was detected.          |
| expected  | string  | Expected value defined by the lint rule.             |
| actual    | string  | Actual value extracted from the document.            |
| message   | string  | Human-readable explanation of the issue.             |


---

### Status Codes

| Code | Description                 |
| ---- | --------------------------- |
| 200  | Lint completed successfully |
| 400  | Invalid request             |
| 413  | File too large              |
| 415  | Unsupported media type      |
| 422  | Invalid or corrupted PDF    |
| 500  | Internal server error       |

---

# Request Validation

Before document extraction begins, the API validates the uploaded file.

Validation rules:

* File is provided.
* File type is PDF.
* File size does not exceed the configured limit.
* PDF is readable.
* PDF is not encrypted.

---

# Error Response

All API errors follow the same structure.

```json
{
  "error": {
    "code": "INVALID_FILE",
    "message": "Only PDF files are supported."
  }
}
```

---

## Error Codes

| Code           | Description                               |
| -------------- | ----------------------------------------- |
| INVALID_FILE   | Uploaded file is not a PDF                |
| FILE_TOO_LARGE | Uploaded file exceeds maximum size        |
| INVALID_PDF    | PDF cannot be parsed                      |
| PDF_ENCRYPTED  | Password-protected PDFs are not supported |
| INTERNAL_ERROR | Unexpected server error                   |

---

# Future Endpoints

The following endpoints are not part of the MVP but are considered for future releases.

## Document Management

```http
POST /api/v1/documents

GET /api/v1/documents/{id}

DELETE /api/v1/documents/{id}
```

---

## Lint Report

```http
GET /api/v1/documents/{id}/report
```

---

## Lint Rules

```http
GET /api/v1/lint-rules

POST /api/v1/lint-rules

PUT /api/v1/lint-rules/{id}

DELETE /api/v1/lint-rules/{id}
```

---

# Versioning

The API is versioned using the URL path.

Current version:

```text
/api/v1
```

Breaking changes will be introduced in a new API version.

Example:

```text
/api/v2
```
