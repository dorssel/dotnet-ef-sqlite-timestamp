// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Dorssel.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public class SqliteTimestampMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider relationalAnnotationProvider)
    : SqliteMigrationsSqlGenerator(dependencies, relationalAnnotationProvider)
{
    sealed record TableSchema(string TableName, string? Schema);

    /// <inheritdoc />
    public override IReadOnlyList<MigrationCommand> Generate(IReadOnlyList<MigrationOperation> operations, IModel? model, MigrationsSqlGenerationOptions options)
    {
        ArgumentNullException.ThrowIfNull(operations);

        if (model is null)
        {
            return base.Generate(operations, model, options);
        }

        var tablesAffected = new HashSet<TableSchema>();

        var augmentedOperations = new List<MigrationOperation>(operations);
        foreach (var operation in operations)
        {
            switch (operation)
            {
                case TableOperation tableOperation:
                    if (operation is RenameTableOperation renameTableOperation)
                    {
                        tablesAffected.Add(new(renameTableOperation.NewName ?? renameTableOperation.Name, renameTableOperation.NewSchema));
                    }
                    tablesAffected.Add(new(tableOperation.Name, tableOperation.Schema));
                    break;

                case ColumnOperation columnOperation:
                    tablesAffected.Add(new(columnOperation.Table, columnOperation.Schema));
                    break;
            }
        }

        foreach (var affected in tablesAffected)
        {
            var delimitedTriggerName = Dependencies.SqlGenerationHelper.DelimitIdentifier(affected.TableName + "_TimestampUpdater", affected.Schema);
            augmentedOperations.Add(new SqlOperation()
            {
                Sql = $"DROP TRIGGER IF EXISTS {delimitedTriggerName};"
            });
            if (model.GetEntityTypes().SingleOrDefault(et => et.GetTableName() == affected.TableName && et.GetSchema() == affected.Schema) is not IEntityType entityType)
            {
                // Table not found; assume it was dropped.
                continue;
            }
            if (entityType.GetProperties().SingleOrDefault(p => p.IsConcurrencyToken && p.ValueGenerated == ValueGenerated.OnAddOrUpdate) is not IProperty property)
            {
                // No [Timestamp] column found.
                continue;
            }
            var delimitedTableName = Dependencies.SqlGenerationHelper.DelimitIdentifier(affected.TableName);
            var delimitedColumnName = Dependencies.SqlGenerationHelper.DelimitIdentifier(property.Name);
            augmentedOperations.Add(new SqlOperation()
            {
                Sql = $"""
                    CREATE TRIGGER {delimitedTriggerName} AFTER UPDATE ON {delimitedTableName}
                    BEGIN
                        UPDATE {delimitedTableName}
                        SET {delimitedColumnName} = {delimitedColumnName} + 1
                        WHERE rowid = NEW.rowid;
                    END;
                    """
            });
        }

        return base.Generate(augmentedOperations, model, options);
    }
}
