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
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            using var db = new NonSqliteDbContext();
            _ = db.Model;
        });
    }
}
