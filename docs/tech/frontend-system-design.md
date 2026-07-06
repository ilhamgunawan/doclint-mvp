# Frontend System Design

# Background

The DocLint frontend provides a web interface for users to upload PDF documents, execute document linting, and review lint reports.

The MVP focuses on a simple and responsive user experience with minimal navigation. Since all document processing is performed by the backend, the frontend acts as a presentation layer responsible for user interactions and API communication.

---

# Scope

The MVP frontend supports:

* Uploading PDF documents
* Displaying upload progress
* Displaying lint status
* Displaying document metadata
* Displaying lint rule results
* Displaying lint issues

---

# Out of Scope

The following features are not included in the MVP:

* User authentication
* User profile
* Document history
* Organization management
* Rule management
* PDF annotation

---

# Tech Stack

| Category         | Technology   |
| ---------------- | ------------ |
| Library          | Vue 3        |
| Language         | TypeScript   |
| Build Tool       | Vite         |
| Router           | Vue Router   |
| State Management | Pinia        |
| HTTP Client      | Axios        |
| UI Components    | PrimeVue     |
| Styling          | Tailwind CSS |
| Icons            | PrimeIcons   |

---

# Use Case Diagram

```text
                +-----------+
                |   User    |
                +-----+-----+
                      |
      +---------------+----------------+
      |                                |
      | Upload PDF                     |
      |                                |
      v                                |
+----------------------+               |
| Submit for Linting   |               |
+----------+-----------+               |
           |                           |
           v                           |
+----------------------+               |
| View Lint Report     |               |
+----------+-----------+               |
           |                           |
           +---------------------------+
                       |
                       v
            Review Lint Issues
```

---

# API Communication

The frontend communicates with the backend through a REST API.

## Request Flow

```text
+------+
| User |
+-+-+--+
  | ^
  v |
+-+-+------------+
| Vue Components |
+-+-+------------+
  | ^
  v |
+-+-+------------+
| Pinia Store    |
+-+-+------------+
  | ^
  v |
+-+-+--------------+
| Document Service |
+-+-+--------------+
  | ^
  v |
+-+-+----------+
| Axios Client |
+-+-+----------+
  | ^
  v |
+-+-+---------+
| Backend API |
+-------------+

```

---

## API Endpoint

### Upload and Lint Document

```http
POST /api/v1/documents/lint
```

Request:

* multipart/form-data

Field:

* file

Response:

* Document metadata
* Summary
* Rule results
* Issues

---

# Routing

The MVP uses a minimal routing structure.

| Route  | Description                        |
| -----  | ---------------------------------- |
| /      | Redirect to `/lint`                |
| /lint  | Upload PDF and display lint report |
| /about | App information                    |
| /404   | Not Found page                     |

Proposed Future routes:

| Route          | Description          |
| -------------- | -------------------- |
| /login         | User authentication  |
| /documents     | Document history     |
| /documents/:id | Document details     |
| /rules         | Lint rule management |
| /settings      | User settings        |

---

# Page Structure

## Lint Page

The Lint page consists of four sections.

```text
+------------------------------------------------------+
| Header                                               |
+------------------------------------------------------+

+------------------------------------------------------+
| Upload Card                                          |
|                                                      |
| [ Select PDF ] [ Run Lint ]                          |
+------------------------------------------------------+

+------------------------------------------------------+
| Document Summary                                     |
|                                                      |
| Status                                               |
| File Name                                            |
| Page Count                                           |
| Rule Count                                           |
| Issue Count                                          |
+------------------------------------------------------+

+------------------------------------------------------+
| Rule Results                                         |
|                                                      |
| ✓ Font Size                                          |
| ✗ Margin                                             |
| ✓ Page Size                                          |
+------------------------------------------------------+

+------------------------------------------------------+
| Lint Issues                                          |
|                                                      |
| Page 4                                               |
| Font Size                                            |
| Expected: 12 pt                                      |
| Actual: 11 pt                                        |
+------------------------------------------------------+
```

---

# State Management

Pinia is used to manage application state.

The MVP consists of a single store.

```text
LintStore

├── document
├── summary
├── ruleResults
├── issues
├── isUploading
├── isLinting
└── error
```

---

# Component Structure

```text
App

└── LintPage
    │
    ├── UploadCard
    ├── DocumentSummary
    ├── RuleResultList
    ├── RuleResultItem
    ├── IssueList
    ├── IssueCard
    └── LoadingOverlay
```

Each component has a single responsibility and receives data via props or Pinia state.

---

# Error Handling

The frontend handles the following scenarios:

| Scenario         | Behaviour                     |
| ---------------- | ----------------------------- |
| Unsupported file | Display validation error      |
| File too large   | Display validation error      |
| Network failure  | Display retry message         |
| Server error     | Display generic error message |
| Invalid PDF      | Display parsing error         |

---

# Future Enhancements

The following enhancements are planned for future releases:

* PDF preview with issue highlighting
* Filter issues by lint rule
* Search lint issues
* Download lint report
* Document history
* Authentication
* Organization support
* Rule configuration
