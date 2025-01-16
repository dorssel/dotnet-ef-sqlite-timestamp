<!--
SPDX-FileCopyrightText: 2024 Frans van Dorsselaer

SPDX-License-Identifier: MIT
-->

# dotnet-ef-sqlite-timestamp

[![Build](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions?query=workflow%3ABuild+branch%3Amain)
[![CodeQL](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions/workflows/codeql.yml/badge.svg?branch=main)](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions?query=workflow%3ACodeQL+branch%3Amain)
[![MegaLinter](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions/workflows/lint.yml/badge.svg?branch=main)](https://github.com/dorssel/dotnet-ef-sqlite-timestamp/actions?query=workflow%3ALint+branch%3Amain)
[![Codecov](https://codecov.io/gh/dorssel/dotnet-ef-sqlite-timestamp/branch/main/graph/badge.svg?token=zsbTiXoisQ)](https://codecov.io/gh/dorssel/dotnet-ef-sqlite-timestamp)
[![REUSE status](https://api.reuse.software/badge/github.com/dorssel/dotnet-ef-sqlite-timestamp)](https://api.reuse.software/info/github.com/dorssel/dotnet-ef-sqlite-timestamp)
[![NuGet](https://img.shields.io/nuget/v/Dorssel.EntityFrameworkCore.Sqlite.Timestamp?logo=nuget)](https://www.nuget.org/packages/Dorssel.EntityFrameworkCore.Sqlite.Timestamp)

Extension for Entity Framework (EF) Core 8.0 (or higher) that adds support for SQLite row versioning (`[Timestamp]` attribute).

The implementation is for AnyCPU, and works on all platforms.

# Usage

Add a package reference to your project for
[`Dorssel.EntityFrameworkCore.Sqlite.Timestamp`](https://www.nuget.org/packages/Dorssel.EntityFrameworkCore.Sqlite.Timestamp),
next to an explicit reference to the required version of SQLite.

For EF Core 8:

```diff
   <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.*" />
+  <PackageReference Include="Dorssel.EntityFrameworkCore.Sqlite.Timestamp" Version="*" />
```

For EF Core 9:

```diff
   <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.*" />
+  <PackageReference Include="Dorssel.EntityFrameworkCore.Sqlite.Timestamp" Version="*" />
```

> [!NOTE]
> It is not recommended to solely rely on the transitive dependency on `Microsoft.EntityFrameworkCore.Sqlite`,
> as that would use the version at the time `Dorssel.EntityFrameworkCore.Sqlite.Timestamp` was built
> instead of the latest one. Therefore, always add an explicit package reference for `Microsoft.EntityFrameworkCore.Sqlite`
> in your project as well.

In your `DbContext` derived class modify the following:

```diff
+  using Dorssel.EntityFrameworkCore;

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

## Example

See the Example project for a demonstration using .NET 9 & EF Core 9.

# NuGet package

The released [NuGet package](https://www.nuget.org/packages/Dorssel.EntityFrameworkCore.Sqlite.Timestamp)
and the .NET assemblies contained therein have the following properties:

- [Strong Naming](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/strong-naming)
- [SourceLink](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/sourcelink)
- [IntelliSense](https://learn.microsoft.com/en-us/visualstudio/ide/using-intellisense)
- [Authenticode](https://learn.microsoft.com/en-us/windows/win32/seccrypto/time-stamping-authenticode-signatures#a-brief-introduction-to-authenticode)
