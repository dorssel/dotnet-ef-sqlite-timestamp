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
                    _ = tablesAffected.Add(new(createTableOperation.Name, createTableOperation.Schema));
                    break;
                case AlterTableOperation alterTableOperation:
                    _ = tablesAffected.Add(new(alterTableOperation.Name, alterTableOperation.Schema));
                    _ = tablesAffected.Add(new(alterTableOperation.OldTable.Name, alterTableOperation.OldTable.Schema));
                    break;
                case RenameTableOperation renameTableOperation:
                    _ = tablesAffected.Add(new(renameTableOperation.NewName ?? renameTableOperation.Name, renameTableOperation.NewSchema));
                    _ = tablesAffected.Add(new(renameTableOperation.Name, renameTableOperation.Schema));
                    break;
                case DropTableOperation dropTableOperation:
                    _ = tablesAffected.Add(new(dropTableOperation.Name, dropTableOperation.Schema));
                    break;

                case AddColumnOperation addColumnOperation:
                    _ = tablesAffected.Add(new(addColumnOperation.Table, addColumnOperation.Schema));
                    break;
                case AlterColumnOperation alterColumnOperation:
                    _ = tablesAffected.Add(new(alterColumnOperation.Table, alterColumnOperation.Schema));
                    break;
                case RenameColumnOperation renameColumnOperation:
                    _ = tablesAffected.Add(new(renameColumnOperation.Table, renameColumnOperation.Schema));
                    break;
                case DropColumnOperation dropColumnOperation:
                    _ = tablesAffected.Add(new(dropColumnOperation.Table, dropColumnOperation.Schema));
                    break;
                default:
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
            //
            // In order to support both Table Splitting and Entity Splitting, we have to consider all
            // properties of all entity types. Of all the [Timestamp] properties that map to our affected table,
            // we find the distinct column names.
            //
            // See https://learn.microsoft.com/en-us/ef/core/modeling/table-splitting
            //
            var columnNames = model.GetEntityTypes().SelectMany(et => et.GetProperties())
                .Where(p => p.IsConcurrencyToken && (p.ValueGenerated == ValueGenerated.OnAddOrUpdate))
                .SelectMany(p => p.GetTableColumnMappings()).Select(cm => cm.Column)
                .Where(c => c.Table.Name == affected.TableName && c.Table.Schema == affected.Schema)
                .Select(c => c.Name).Distinct().ToArray();
            switch (columnNames.Length)
            {
                case 0:
                    // Table not found (dropped), or it does not (any longer) contain a [Timestamp] column.
                    continue;
                default:
                    // SQL Server (as do we) only supports one row version column per table, see
                    //    https://learn.microsoft.com/en-us/sql/t-sql/data-types/rowversion-transact-sql
                    throw new InvalidOperationException("Only one row version column per table is supported.");
                case 1:
                    break;
            }
            var delimitedTableName = Dependencies.SqlGenerationHelper.DelimitIdentifier(affected.TableName);
            var delimitedColumnName = Dependencies.SqlGenerationHelper.DelimitIdentifier(columnNames[0]);
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
