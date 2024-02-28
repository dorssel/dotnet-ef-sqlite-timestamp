// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Dorssel.EntityFrameworkCore.Infrastructure;
using Dorssel.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dorssel.EntityFrameworkCore;

public static class SqliteTimestampDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseSqliteTimestamp(this DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        optionsBuilder.ReplaceService<IMigrationsSqlGenerator, SqliteTimestampMigrationsSqlGenerator>();
        optionsBuilder.ReplaceService<IModelCustomizer, SqliteTimestampModelCustomizer>();
        return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> UseSqliteTimestamp<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder) where TContext : DbContext
    {
        return (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)optionsBuilder).UseSqliteTimestamp();
    }
}
