// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Dorssel.EntityFrameworkCore.Storage.ValueConversion;

namespace UnitTests;

[TestClass]
sealed class TimestampToLongConverter_Tests
{
    static IEnumerable<object[]> KnownGoodConversions => [
            [0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }],
            [1, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }],
            [long.MaxValue, new byte[] { 0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }],
            [-1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }],
            [long.MinValue, new byte[] { 0x80, 0, 0, 0, 0, 0, 0, 0 }],
        ];

    [TestMethod]
    [DynamicData(nameof(KnownGoodConversions))]
    public void TimestampToLongConverter_ConvertToProviderTyped(long expectedLong, byte[] expectedTimestamp)
    {
        var result = TimestampToLongConverter.Singleton.ConvertToProviderTyped(expectedTimestamp);
        Assert.AreEqual(expectedLong, result);
    }

    [TestMethod]
    [DynamicData(nameof(KnownGoodConversions))]
    public void TimestampToLongConverter_ConvertFromProviderTyped(long expectedLong, byte[] expectedTimestamp)
    {
        var result = TimestampToLongConverter.Singleton.ConvertFromProviderTyped(expectedLong);
        CollectionAssert.AreEqual(expectedTimestamp, result);
    }
}
