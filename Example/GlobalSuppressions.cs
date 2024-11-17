// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods",
    Justification = "Allow auto-generated migrations to pass unmodified.", Scope = "namespaceanddescendants", Target = "~N:Example.Migrations")]
[assembly: SuppressMessage("Style", "IDE0161:Convert to file-scoped namespace",
    Justification = "Allow auto-generated migrations to pass unmodified.", Scope = "namespaceanddescendants", Target = "~N:Example.Migrations")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used",
    Justification = "Allow auto-generated migrations to pass unmodified.", Scope = "namespaceanddescendants", Target = "~N:Example.Migrations")]
[assembly: SuppressMessage("Maintainability", "CA1515:Consider making public types internal",
    Justification = "Allow auto-generated migrations to pass unmodified.", Scope = "namespaceanddescendants", Target = "~N:Example.Migrations")]
