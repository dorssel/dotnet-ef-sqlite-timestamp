// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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
                case CreateTableOperation createTableOperation:
                    tablesAffected.Add(new(createTableOperation.Name, createTableOperation.Schema));
                    break;
                case AlterTableOperation alterTableOperation:
                    tablesAffected.Add(new(alterTableOperation.Name, alterTableOperation.Schema));
                    tablesAffected.Add(new(alterTableOperation.OldTable.Name, alterTableOperation.OldTable.Schema));
                    break;
                case RenameTableOperation renameTableOperation:
                    tablesAffected.Add(new(renameTableOperation.NewName ?? renameTableOperation.Name, renameTableOperation.NewSchema));
                    tablesAffected.Add(new(renameTableOperation.Name, renameTableOperation.Schema));
                    break;
                case DropTableOperation dropTableOperation:
                    tablesAffected.Add(new(dropTableOperation.Name, dropTableOperation.Schema));
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
            if (model.GetEntityTypes().FirstOrDefault(et => et.GetTableName() == affected.TableName && et.GetSchema() == affected.Schema) is not IEntityType entityType)
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
