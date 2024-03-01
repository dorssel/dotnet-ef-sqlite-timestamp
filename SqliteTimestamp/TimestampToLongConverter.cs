// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Buffers.Binary;

namespace Dorssel.EntityFrameworkCore.Storage.ValueConversion;

/// <inheritdoc />
public sealed class TimestampToLongConverter : ValueConverter<byte[], long>
{
    /// <summary>
    /// The singeleton instance of this class.
    /// </summary>
    public static readonly TimestampToLongConverter Singleton = new();

    TimestampToLongConverter()
        : base(v => BinaryPrimitives.ReadInt64BigEndian(v), (v) => ToByteArray(v))
    {
    }

    static byte[] ToByteArray(long v)
    {
        var result = new byte[8];
        BinaryPrimitives.WriteInt64BigEndian(result, v);
        return result;
    }
}
