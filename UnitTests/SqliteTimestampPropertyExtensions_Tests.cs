// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore.Metadata;

namespace UnitTests;

[TestClass]
sealed class SqliteTimestampPropertyExtensions_Tests
{
    const string EntityName = "Entity";
    const string PropertyName = "Property";

    [TestMethod]
    public void NullThrows()
    {
        IReadOnlyProperty property = null!;
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            _ = property.IsSqliteTimestamp();
        });
    }

    [TestMethod]
    public void Timestamp_True()
    {
        var modelBuilder = new ModelBuilder();

        modelBuilder.Entity(EntityName).Property<byte[]>(PropertyName)
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();

        var model = modelBuilder.FinalizeModel();

        var property = model.FindEntityType(EntityName)!.FindProperty(PropertyName)!;

        Assert.IsTrue(property.IsSqliteTimestamp());
    }

    [TestMethod]
    public void WrongClrType_False()
    {
        var modelBuilder = new ModelBuilder();

        modelBuilder.Entity(EntityName).Property<long>(PropertyName)
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();

        var model = modelBuilder.FinalizeModel();

        var property = model.FindEntityType(EntityName)!.FindProperty(PropertyName)!;

        Assert.IsFalse(property.IsSqliteTimestamp());
    }

    [TestMethod]
    public void WrongValueGenerated_False()
    {
        var modelBuilder = new ModelBuilder();

        modelBuilder.Entity(EntityName).Property<byte[]>(PropertyName)
            .ValueGeneratedOnUpdate()
            .IsConcurrencyToken();

        var model = modelBuilder.FinalizeModel();

        var property = model.FindEntityType(EntityName)!.FindProperty(PropertyName)!;

        Assert.IsFalse(property.IsSqliteTimestamp());
    }

    [TestMethod]
    public void NoConcurrencyToken_False()
    {
        var modelBuilder = new ModelBuilder();

        modelBuilder.Entity(EntityName).Property<byte[]>(PropertyName)
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken(false);

        var model = modelBuilder.FinalizeModel();

        var property = model.FindEntityType(EntityName)!.FindProperty(PropertyName)!;

        Assert.IsFalse(property.IsSqliteTimestamp());
    }
}
