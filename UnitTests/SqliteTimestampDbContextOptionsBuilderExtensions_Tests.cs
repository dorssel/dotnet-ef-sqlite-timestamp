// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

namespace UnitTests;

[TestClass]
sealed class SqliteTimestampDbContextOptionsBuilderExtensions_Tests
{
    [TestMethod]
    public void UseSqliteTimestamp()
    {
        var builder = new DbContextOptionsBuilder();
        _ = builder.UseSqliteTimestamp();
    }

    [TestMethod]
    public void UseSqliteTimestamp_Generic()
    {
        var builder = new DbContextOptionsBuilder<DbContext>();
        _ = builder.UseSqliteTimestamp();
    }
}
