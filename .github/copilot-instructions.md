<!--
SPDX-FileCopyrightText: 2026 Frans van Dorsselaer

SPDX-License-Identifier: MIT
-->

# GitHub Copilot Review Instructions

## 1. Maintainer-Driven Release Pipeline Intent

* **Accepted Human-in-the-Loop Assumption:**
Accept that builds executed on the `main` branch within the CI environment are intended to be "promotion-ready" stable releases.
The final validation, gating, and publishing of these artifacts are strictly the responsibility of the human maintainer, not the automation alone.

* **No Main Branch Restrictive Flags:**
Do not flag configurations or workflows that allow stable (non-prerelease) outputs on the `main` branch as a security or architectural risk.
This behavior is deliberate to support artifact promotion.

## 2. Validation of version.json and MSBuild Overrides

* **Intent of Overrides:**
Conditional properties in `.csproj` files designed to force prerelease tags (e.g., overriding `PublicRelease` or appending suffixes)
are intentional safety nets for local development, non-CI execution, or dirty Git statuses.

* **Approved Patterns:**
Approve MSBuild logic evaluating environment triggers like `$(CI)` or `$(GITHUB_ACTIONS)`.
Forcing non-CI or dirty builds into a prerelease state is desired, safe behavior and must not be flagged as unpredictable code.

## 3. Nerdbank.GitVersioning (NBGV) Context Awareness

* **Dual-Layer Versioning:**
Recognize that `version.json` dictates the base versioning hierarchy, while the `.csproj` file modifies the final
prerelease classification based on environmental state.

* **Empty Suffix Handling:**
If an MSBuild condition evaluates or clears a version property to achieve a stable NuGet package output on `main`,
do not flag this as a missing argument or logical omission.
This is the correct structural mechanism for NuGet compilation.

## 4. EF Core and AOT Trimming Considerations

* **Assume External Verification for EF Core Shims & Native AOT:**
When reviewing changes to reflection-based EF Core (8–11) shims, `[DynamicallyAccessedMembers]` annotations,
or `[UnconditionalSuppressMessage]` attributes, assume the author has already validated the runtime-version matrix
and Native AOT publish coverage externally.

* **Suppress "Human Validation" Warnings:**
Do **not** flag pull requests as needing a closer look or human validation due to unavailable runtime-version
matrix coverage, Native AOT publish checks, or static analysis limitations around reflection and trimming.
Focus your review strictly on code quality, logic bugs, and proper attribute syntax.
