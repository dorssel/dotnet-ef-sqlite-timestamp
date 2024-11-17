// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Dorssel.EntityFrameworkCore.Infrastructure;
using Dorssel.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dorssel.EntityFrameworkCore;

/// <inheritdoc cref="SqliteDbContextOptionsBuilderExtensions" />
public static class SqliteTimestampDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Adds support for <see cref="System.ComponentModel.DataAnnotations.TimestampAttribute" /> to an SQLite database context.
    /// </summary>
    public static DbContextOptionsBuilder UseSqliteTimestamp(this DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        _ = optionsBuilder.ReplaceService<IMigrationsSqlGenerator, SqliteTimestampMigrationsSqlGenerator>();
        _ = optionsBuilder.ReplaceService<IModelCustomizer, SqliteTimestampModelCustomizer>();
        return optionsBuilder;
    }

    /// <inheritdoc cref="UseSqliteTimestamp(DbContextOptionsBuilder)" />
    public static DbContextOptionsBuilder<TContext> UseSqliteTimestamp<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder) where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)optionsBuilder).UseSqliteTimestamp();
    }
}
