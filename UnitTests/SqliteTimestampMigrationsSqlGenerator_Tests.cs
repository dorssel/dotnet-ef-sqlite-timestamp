// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace UnitTests;

[TestClass]
sealed class SqliteTimestampMigrationsSqlGenerator_Tests
{
    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_CreateTable()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new CreateTableOperation() { Name = MemoryDbContext.TestTableName }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_RenameTable()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new RenameTableOperation() { Name = MemoryDbContext.TestTableName, NewName = "Renamed" }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_RenameSchema()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new RenameTableOperation() { Name = MemoryDbContext.TestTableName, NewSchema = "Renamed" }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_AlterTable()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new AlterTableOperation() { Name = MemoryDbContext.TestTableName }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_DropTable()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new DropTableOperation() { Name = MemoryDbContext.TestTableName }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_AddColumn()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new AddColumnOperation() { Table = MemoryDbContext.TestTableName, Name = "NewColumn", ClrType = typeof(int) }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_AlterColumn()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new AlterColumnOperation() { Table = MemoryDbContext.TestTableName, Name = MemoryDbContext.RowVersionName }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_RenameColumn()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new RenameColumnOperation() { Table = MemoryDbContext.TestTableName, Name = MemoryDbContext.RowVersionName, NewName = "NewRowVersion" }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_DropColumn()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new DropColumnOperation() { Table = MemoryDbContext.TestTableName }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsTrue(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_OtherOperation()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new DeleteDataOperation()
            {
                Table = MemoryDbContext.TestTableName,
                KeyColumns = [MemoryDbContext.IdName],
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
                KeyValues = new object[,] { { 1 } },
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional
            }
        };

        var result = generator.Generate(operations, designTimeModel.Model);

        Assert.IsFalse(result.Any(c => c.CommandText.Contains("TRIGGER")));
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_NullModelNoThrow()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();

        generator.Generate([]);
    }

    [TestMethod]
    public void SqliteTimestampMigrationsSqlGenerator_MultipleRowVersionsThrows()
    {
        using var db = new MultipleRowVersionsDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();
        var designTimeModel = db.GetService<IDesignTimeModel>();
        var operations = new List<MigrationOperation>
        {
            new AlterTableOperation() { Name = MemoryDbContext.TestTableName }
        };

        Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            generator.Generate(operations, designTimeModel.Model);
        });
    }
}
