// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

namespace UnitTests;

[TestClass]
sealed class Api_Tests
{
    [TestMethod]
    public void UseSqliteTimestamp_NullBuilderThrows()
    {
        DbContextOptionsBuilder optionsBuilder = null!;
        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            optionsBuilder.UseSqliteTimestamp();
        });
    }
}
