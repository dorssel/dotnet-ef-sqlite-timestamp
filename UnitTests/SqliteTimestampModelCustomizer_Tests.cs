// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

namespace UnitTests;

[TestClass]
sealed class SqliteTimestampModelCustomizer_Tests
{
    [TestMethod]
    public void SqliteTimestampModelCustomizer_Customize()
    {
        using var db = new MemoryDbContext();

        var TestTable = db.Model.FindEntityType(MemoryDbContext.TestTableName)!;
        var RowVersion = TestTable.FindProperty(MemoryDbContext.RowVersionName)!;

        Assert.AreEqual(typeof(long), RowVersion.GetProviderClrType());
    }

    [TestMethod]
    public void SqliteTimestampModelCustomizer__MultipleRowVersionsSupported()
    {
        using var db = new MultipleRowVersionsDbContext();

        var TestTable = db.Model.FindEntityType(MemoryDbContext.TestTableName)!;
        var RowVersion = TestTable.FindProperty(MemoryDbContext.RowVersionName)!;
        var SecondRowVersion = TestTable.FindProperty(MultipleRowVersionsDbContext.SecondRowVersionName)!;

        Assert.AreEqual(typeof(long), RowVersion.GetProviderClrType());
        Assert.AreEqual(typeof(long), SecondRowVersion.GetProviderClrType());
    }

    sealed class NonSqliteDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseInMemoryDatabase("Test")
                .UseSqliteTimestamp();
        }
    }

    [TestMethod]
    public void SqliteTimestampModelCustomizer_NonSqliteThrows()
    {
        using var db = new NonSqliteDbContext();
        Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            _ = db.Model;
        });
    }
}
