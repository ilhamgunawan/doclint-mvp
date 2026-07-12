# Backend Processing Pipeline

## Background

The DocLint backend is responsible for transforming an uploaded PDF document into a structured lint report.

To achieve this, the backend is divided into two independent processing pipelines:

1. **Document Processing Pipeline** – extracts and normalizes information from a PDF into a common document model.
2. **Linting Pipeline** – evaluates the document model against predefined lint rules and produces a lint report.

This separation allows the document extraction logic and linting logic to evolve independently.

---

# Architecture Overview

```text
                +-------------------+
                |     REST API      |
                +---------+---------+
                          |
                          v
                +-------------------+
                | DocumentLintService|
                +---------+---------+
                          |
            +-------------+-------------+
            |                           |
            v                           v
+-------------------------+   +----------------------+
| Document Processing     |   |  Linting Pipeline    |
|        Pipeline         |   |                      |
+------------+------------+   +-----------+----------+
             |                            ^
             |                            |
             +-------- DocumentModel ------+
                          |
                          v
                    Lint Report
                          |
                          v
                      REST Response
```

---

# Processing Flow

```text
Upload PDF
    │
    ▼
REST API
    │
    ▼
DocumentLintService
    │
    ▼
Document Processing Pipeline
    │
    ▼
DocumentModel
    │
    ▼
Linting Pipeline
    │
    ▼
LintReport
    │
    ▼
Response JSON
```

---

# Document Processing Pipeline

## Purpose

Convert a PDF document into a normalized `DocumentModel`.

The rest of the application should never depend directly on the PDF processing library.

---

## Components

### Document Extractor

Acts as the entry point of the document processing pipeline.

Responsibilities:

* Coordinate document extraction
* Build the `DocumentModel`
* Hide implementation details of the PDF library

Interface:

```csharp
IDocumentExtractor
```

---

### Metadata Extractor

Extracts document-level metadata.

Responsibilities:

* File name
* File size
* MIME type
* Page count
* Page size
* Page orientation

Output:

```text
DocumentMetadata
```

---

### Content Extractor

Extracts page content.

Responsibilities:

* Text
* Font family
* Font size
* Bounding box
* Coordinates
* Page information

Output:

```text
DocumentContent
```

---

### Document Model

The normalized representation used by the entire application.

The `DocumentModel` combines:

* Document metadata
* Pages
* Text blocks
* Fonts
* Positions

All downstream services consume this model instead of interacting with the PDF library.

---

# Linting Pipeline

## Purpose

Evaluate the extracted document against predefined lint rules.

---

## Components

### Rule Engine

Coordinates lint rule execution.

Responsibilities:

* Discover lint rules
* Execute each rule
* Collect rule results
* Aggregate lint issues

The Rule Engine does not implement validation logic.

---

### Lint Rules

Each lint rule validates a single aspect of the document.

Examples:

* Font Size Rule
* Font Family Rule
* Margin Rule
* Page Size Rule
* Page Orientation Rule

Each rule returns:

* Rule status
* Rule issues

Rules are independent and can be added or removed without affecting the Rule Engine.

---

### Lint Report

Aggregates the final linting result.

Contains:

* Document
* Summary
* Rule Results
* Issues

This object is returned directly to the REST API.

---

# Backend Responsibilities

## REST API

Responsibilities:

* Accept file uploads
* Validate requests
* Invoke `DocumentLintService`
* Return HTTP responses

---

## DocumentLintService

Acts as the application orchestrator.

Responsibilities:

1. Receive uploaded PDF
2. Execute the Document Processing Pipeline
3. Execute the Linting Pipeline
4. Return the final lint report

The service should not contain document parsing or lint rule implementations.

---

# Folder Structure

```text
Application
├── Interfaces
│   ├── IDocumentExtractor.cs
│   └── ILintRule.cs
│
├── Models
│   ├── DocumentModel.cs
│   └── LintReport.cs
│
├── Services
│   └── DocumentLintService.cs
│
└── Rules

Infrastructure
├── DocumentProcessing
│   └── PdfPig
│       ├── PdfPigDocumentExtractor.cs
│       ├── MetadataExtractor.cs
│       ├── ContentExtractor.cs
│       └── PdfPigMapper.cs
│
└── DependencyInjection.cs
```

---

# Dependency Flow

```text
REST API
    │
    ▼
DocumentLintService
    │
    ▼
IDocumentExtractor
    │
    ▼
PdfPigDocumentExtractor
    │
    ▼
DocumentModel
    │
    ▼
RuleEngine
    │
    ▼
ILintRule
    │
    ▼
LintReport
```

Dependencies always point inward following the principles of Clean Architecture.

---

# Design Principles

The backend architecture follows these principles:

* Clean Architecture
* Single Responsibility Principle (SRP)
* Dependency Inversion Principle (DIP)
* Stateless request processing
* Pipeline-oriented processing
* Extensible document extractors
* Extensible lint rule architecture
