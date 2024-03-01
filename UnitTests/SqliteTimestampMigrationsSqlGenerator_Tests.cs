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
    public void SqliteTimestampMigrationsSqlGenerator_NullModelNoThrow()
    {
        using var db = new MemoryDbContext();
        var generator = db.GetService<IMigrationsSqlGenerator>();

        _ = generator.Generate([]);
    }
}
