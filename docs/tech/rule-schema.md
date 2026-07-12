# Rule Schema

## Overview

This document defines the JSON schema used by DocLint to configure lint rules.

During the MVP stage, lint rules are stored as static JSON files and loaded by the backend during application startup.

Each rule defines:

* what should be validated
* where the validation should be applied
* the expected values
* the severity level
* whether the rule is enabled

Future versions may store rules in a database without changing the logical structure defined in this document.

---

# Root Structure

The root object contains a collection of lint rules.

```json
{
  "rules": []
}
```

| Field | Type  | Required | Description               |
| ----- | ----- | -------- | ------------------------- |
| rules | array | Yes      | Collection of lint rules. |

---

# Rule

A rule represents a single validation rule executed by the Lint Engine.

```json
{
  "id": 1,
  "name": "Cover Font",
  "type": "font",
  "constraints": {},
  "pageSelector": {},
  "severity": "warning",
  "isActive": true
}
```

| Field        | Type    | Required | Description                                  |
| ------------ | ------- | -------- | -------------------------------------------- |
| id           | integer | Yes      | Unique rule identifier.                      |
| name         | string  | Yes      | Human-readable rule name.                    |
| type         | string  | Yes      | Validation type executed by the Lint Engine. |
| constraints  | object  | Yes      | Expected values used during validation.      |
| pageSelector | object  | Yes      | Defines which pages the rule applies to.     |
| severity     | string  | Yes      | Severity level (`error`, `warning`).         |
| isActive     | boolean | Yes      | Indicates whether the rule is enabled.       |

---

# Rule Types

The following rule types are supported in the MVP.

| Type             | Description                         |
| ---------------- | ----------------------------------- |
| font             | Validate font family and font size. |
| page_margin      | Validate page margins.              |
| page_size        | Validate page dimensions.           |
| page_orientation | Validate page orientation.          |

---

# Constraints

The structure of `constraints` depends on the rule type.

## Font

```json
{
  "type": "font",
  "constraints": {
    "fontFamily": "arial",
    "fontSize": {
      "value": 12,
      "unit": "pt"
    }
  }
}
```

| Field          | Type   | Description           |
| -------------- | ------ | --------------------- |
| fontFamily     | string | Expected font family. |
| fontSize.value | number | Expected font size.   |
| fontSize.unit  | string | Unit of measurement.  |

---

## Page Margin

```json
{
  "type": "page_margin",
  "constraints": {
    "top": {
      "value": 12,
      "unit": "cm"
    },
    "bottom": {
      "value": 12,
      "unit": "cm"
    },
    "left": {
      "value": 10,
      "unit": "cm"
    },
    "right": {
      "value": 10,
      "unit": "cm"
    }
  }
}
```

| Field  | Description    |
| ------ | -------------- |
| top    | Top margin.    |
| bottom | Bottom margin. |
| left   | Left margin.   |
| right  | Right margin.  |

Each margin consists of:

| Field | Type   | Description           |
| ----- | ------ | --------------------- |
| value | number | Expected measurement. |
| unit  | string | Measurement unit.     |

---

## Page Size

```json
{
  "type": "page_size",
  "constraints": {
    "width": {
      "value": 595,
      "unit": "pt"
    },
    "height": {
      "value": 842,
      "unit": "pt"
    }
  }
}
```

| Field  | Description           |
| ------ | --------------------- |
| width  | Expected page width.  |
| height | Expected page height. |

Each dimension consists of:

| Field | Type   | Description           |
| ----- | ------ | --------------------- |
| value | number | Expected measurement. |
| unit  | string | Measurement unit.     |

---

## Page Orientation

```json
{
  "type": "page_orientation",
  "constraints": {
    "orientation": "portrait"
  }
}
```

Supported values:

* portrait
* landscape

---

# Page Selector

Defines which pages should be validated.

## All Pages

```json
{
  "pageSelector": {
    "type": "all"
  }
}
```

---

## Page Range

```json
{
  "pageSelector": {
    "type": "range",
    "start": 2,
    "end": null
  }
}
```

| Field | Type           | Description                                                              |
| ----- | -------------- | ------------------------------------------------------------------------ |
| type  | string         | Selector type (`all`, `range`).                                          |
| start | integer        | Starting page (inclusive).                                               |
| end   | integer | null | Ending page (inclusive). `null` indicates the last page of the document. |

---

# Severity

Determines the impact of a rule violation.

| Value   | Description                         |
| ------- | ----------------------------------- |
| error   | The document fails the rule.        |
| warning | The document passes with a warning. |

---

# Rule Execution

The Lint Engine processes rules using the following workflow:

```text
Load Rules

        │

        ▼

Ignore Inactive Rules

        │

        ▼

Resolve Page Selector

        │

        ▼

Execute Rule Validator

        │

        ▼

Generate Lint Issues

        │

        ▼

Generate Lint Report
```

---

# Example

```json
{
  "rules": [
    {
      "id": 2,
      "name": "Content Font",
      "type": "font",
      "constraints": {
        "fontFamily": "arial",
        "fontSize": {
          "value": 12,
          "unit": "pt"
        }
      },
      "pageSelector": {
        "type": "range",
        "start": 2,
        "end": null
      },
      "severity": "error",
      "isActive": true
    }
  ]
}
```
