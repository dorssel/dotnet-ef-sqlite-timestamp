// SPDX-FileCopyrightText: 2024 Frans van Dorsselaer
//
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Buffers.Binary;

namespace Dorssel.EntityFrameworkCore.Storage.ValueConversion;

public sealed class TimestampToLongConverter : ValueConverter<byte[], long>
{
    public readonly static TimestampToLongConverter Singleton = new();

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
