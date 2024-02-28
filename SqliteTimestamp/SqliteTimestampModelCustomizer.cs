// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Dorssel.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dorssel.EntityFrameworkCore.Infrastructure;

public class SqliteTimestampModelCustomizer(ModelCustomizerDependencies dependencies)
    : RelationalModelCustomizer(dependencies)
{
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        ArgumentNullException.ThrowIfNull(context);

        if (!context.Database.IsSqlite())
        {
            throw new InvalidOperationException("UseSqliteTimestamp must only be used on actual SQLite databases.");
        }

        // This calls OnModelCreating, which may add/modify tables.
        base.Customize(modelBuilder, context);

        // After all modeling is done, we need to fix-up the tables that contain a [Timestamp].
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.GetProperties().SingleOrDefault(p => p.ClrType == typeof(byte[]) && p.IsConcurrencyToken
                && p.ValueGenerated == ValueGenerated.OnAddOrUpdate) is not IMutableProperty property)
            {
                // No [Timestamp] column found.
                continue;
            }
            property.SetColumnType("INTEGER");
            property.SetDefaultValueSql("0");
            property.SetValueConverter(TimestampToLongConverter.Singleton);
            property.SetValueComparer(new ArrayStructuralComparer<byte>());
            entityType.UseSqlReturningClause(false);
        }
    }
}
