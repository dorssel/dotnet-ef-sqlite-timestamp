// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

namespace UnitTests;

[TestClass]
sealed class SqliteTimestampDbContextOptionsBuilderExtensions_Tests
{
    sealed class TestDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqliteTimestamp();
        }
    }

    [TestMethod]
    public void UseSqliteTimestamp()
    {
        using var db = new TestDbContext();
    }
}
