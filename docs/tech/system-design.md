# System Design

## Background

DocLint is a PDF Document Format Validator that analyzes uploaded PDF documents against predefined lint rules.

The goal of the MVP is to provide users with an automated way to detect document formatting issues and produce a lint report containing page-level issue locations.

Unlike editable document formats such as DOCX, PDF documents contain rendered layout information. This allows DocLint to accurately identify the page containing a lint issue without implementing a document layout engine.

---

# Scope

The MVP includes:

* Upload PDF documents
* Extract document metadata
* Extract page information
* Extract text blocks and font information
* Execute predefined lint rules
* Generate lint reports
* Display lint issues with page numbers

---

# Out of Scope

The following features are intentionally excluded from the MVP:

* DOCX support
* User authentication
* Multi-tenant architecture
* Custom lint rule management
* Automatic document correction
* AI-assisted recommendations
* PDF annotation
* Version history

---

# High Level Architecture

```text
                    +-------------------+
                    |        UI         |
                    +---------+---------+
                              |
                              |
                              v
                    +-------------------+
                    |      REST API     |
                    +---------+---------+
                              |
                              |
                              v
                +---------------------------+
                |   Document Extractor      |
                +------------+--------------+
                             |
                             |
                             v
                +---------------------------+
                |     Document Model        |
                +------------+--------------+
                             |
                             |
                             v
                +---------------------------+
                |       Lint Engine         |
                +------------+--------------+
                             |
          +------------------+------------------+
          |                                     |
          v                                     v
        Rule 1                                Rule n
          \                                     /
           \                                   /
            +----------------+----------------+
                             |
                             v
                +---------------------------+
                |       Lint Report         |
                +------------+--------------+
                             |
                             |
                             v
                    +-------------------+
                    |        UI         |
                    +-------------------+
```

---

# Tech Stack

| Layer               | Technology                   |
| ------------------- | ---------------------------- |
| Frontend            | TypeScript & Vue 3           |
| Backend             | C# & ASP.NET Core            |
| API                 | REST                         |
| Document Extraction | PDF processing library (TBD) |
| Database            | PostgreSQL (future)          |
| Storage             | Local Storage (MVP)          |
| Deployment          | Docker                       |

---

# Components

## Frontend

Responsibilities

* Upload PDF
* Display lint progress
* Display lint report
* Navigate to lint issues

---

## REST API

Responsibilities

* Receive uploaded PDF
* Trigger linting process
* Return lint report

---

## Document Extractor

Responsibilities

* Read PDF document
* Extract document metadata
* Extract pages
* Extract text blocks
* Extract font information
* Extract text coordinates
* Build the Document Model

Output

```text
Document
```

---

## Document Model

The Document Model is the internal representation used by all downstream components.

The Lint Engine must not depend on PDF-specific APIs.

Example structure

```text
Document

├── Metadata
│
├── Pages
│   ├── Width
│   ├── Height
│   ├── Rotation
│   ├── Text Blocks
│   └── Images
│
└── Properties
```

---

## Lint Engine

Responsibilities

* Execute predefined lint rules
* Aggregate lint results
* Produce lint report

The Lint Engine is independent of the document source format.

Future document extractors (e.g. DOCX) should be able to reuse the same engine by producing the same Document Model.

---

## Lint Rule

Each lint rule validates exactly one concern.

* Font Family Rule
* Font Size Rule
* Page Margin Rule
* Page Size Rule
* Page Orientation Rule

Each lint rule returns:

* Status
* Message
* Expected value
* Actual value
* Page location

---

## Lint Report

The Lint Report aggregates all lint rule results into a single response.

Example

```text
Lint Status

FAILED

Issues

Page 2
- Font Size
- Expected 12 pt
- Actual 11 pt

Page 5
- Margin
- Expected 2.5 cm
- Actual 2 cm
```

---

# Data Design

## Data Flow

```text
Upload PDF -> REST API -> Document Extractor -> Document Model -> Lint Engine -> Lint Rules -> Lint Report -> UI
```

---

## Document Model

```text
Document

├── Id
├── FileName
├── Metadata
│
├── Pages[]
│
│   ├── PageNumber
│   ├── Width
│   ├── Height
│   ├── Rotation
│   ├── TextBlocks[]
│   │
│   │   ├── Text
│   │   ├── FontFamily
│   │   ├── FontSize
│   │   ├── Position (x, y)
│   │   └── BoundingBox
│   │
│   └── Images[]
│
└── Properties
```

---

## Lint Report

```text
LintReport

├── Status
├── Summary
│
├── Issues[]
│
│   ├── Rule
│   ├── Severity
│   ├── Page
│   ├── Expected
│   ├── Actual
│   └── Message
```

---

## Storage Design

### MVP

No persistent storage is required.

The uploaded PDF is processed in memory (or a temporary file), and the generated lint report is returned immediately to the client.

```text
PDF

↓

Temporary Processing

↓

Lint Report

↓

Response

↓

Delete Temporary File
```

Future versions may introduce persistent storage for uploaded documents, lint reports, user accounts, and organization-specific lint rules.
