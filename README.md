<!--
SPDX-FileCopyrightText: 2024 Frans van Dorsselaer

SPDX-License-Identifier: MIT
-->

# dotnet-ef-sqlite-timestamp

[![Build](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions?query=workflow%3ABuild+branch%3Amain)
[![CodeQL](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions/workflows/codeql.yml/badge.svg?branch=main)](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions?query=workflow%3ACodeQL+branch%3Amain)
[![MegaLinter](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions/workflows/lint.yml/badge.svg?branch=main)](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions?query=workflow%3ALint+branch%3Amain)
[![codecov](https://codecov.io/gh/dorssel/dotnet-ef-sqlite-timestamp/branch/main/graph/badge.svg?token=zsbTiXoisQ)](https://codecov.io/gh/dorssel/dotnet-ef-sqlite-timestamp)
[![REUSE status](https://api.reuse.software/badge/github.com/dorssel/dotnet-ef-sqlite-timestamp)](https://api.reuse.software/info/github.com/dorssel/dotnet-ef-sqlite-timestamp)
[![NuGet](https://img.shields.io/nuget/v/Dorssel.EntityFrameworkCore.Sqlite.Timestamp?logo=nuget)](https://www.nuget.org/packages/Dorssel.EntityFrameworkCore.Sqlite.Timestamp)

Extension for Entity Framework (EF) Core 8.0 that adds support for SQLite row versioning (`[Timestamp]` attribute).

The implementation is for AnyCPU, and works on all platforms.

# Usage

Add a package reference to your project for [`Dorssel.EntityFrameworkCore.Sqlite.Timestamp`](https://www.nuget.org/packages/Dorssel.EntityFrameworkCore.Sqlite.Timestamp).

In your `DbContext` derived class modify the following:

```diff
+using Dorssel.EntityFrameworkCore;

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
     optionsBuilder
       .UseSqlite($"Data Source={DbPath}")
+      .UseSqliteTimestamp();
   }
```

This is all you need to support row versioning with SQLite, including support for migrations.

See [Native database-generated concurrency tokens](https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations#native-database-generated-concurrency-tokens);
except now you can ignore the part that says it isn't supported with SQLite!

The released binary NuGet packages and the .NET assemblies contained therein have the following properties:

- [Strong Naming](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/strong-naming)
- [SourceLink](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink)
- [IntelliSense](https://learn.microsoft.com/en-us/visualstudio/ide/using-intellisense)
- [Authenticode](https://learn.microsoft.com/en-us/windows/win32/seccrypto/time-stamping-authenticode-signatures#a-brief-introduction-to-authenticode)
