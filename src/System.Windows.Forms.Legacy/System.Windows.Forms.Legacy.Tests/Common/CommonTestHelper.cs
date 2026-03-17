// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using System.Windows.Forms;

namespace System.Windows.Forms.Legacy.Tests;

public static class CommonTestHelper
{
    public static TheoryData<bool> GetBoolTheoryData()
        => new()
        {
            true,
            false
        };

    public static TheoryData<int> GetIntTheoryData()
        => new()
        {
            0,
            1,
            -1,
            int.MaxValue,
            int.MinValue
        };

    public static TheoryData<RightToLeft, RightToLeft> GetRightToLeftTheoryData()
        => new()
        {
            { RightToLeft.Inherit, RightToLeft.No },
            { RightToLeft.Yes, RightToLeft.Yes },
            { RightToLeft.No, RightToLeft.No }
        };

    public static TheoryData<string?, string> GetStringNormalizedTheoryData()
        => new()
        {
            { null, string.Empty },
            { string.Empty, string.Empty },
            { "teststring", "teststring" }
        };

    public static TheoryData<string?> GetStringWithNullTheoryData()
        => new()
        {
            (string?)null,
            string.Empty,
            "teststring"
        };

    public static IEnumerable<object?[]> GetEnumTypeTheoryData(Type enumType)
    {
        ArgumentNullException.ThrowIfNull(enumType);

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type must be an enum.", nameof(enumType));
        }

        foreach (object value in Enum.GetValues(enumType))
        {
            yield return [value];
        }
    }

    public static IEnumerable<object?[]> GetEnumTypeTheoryDataInvalid(Type enumType)
    {
        ArgumentNullException.ThrowIfNull(enumType);

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Type must be an enum.", nameof(enumType));
        }

        bool isFlags = enumType.GetCustomAttribute<FlagsAttribute>() is not null;

        if (isFlags)
        {
            object? firstUndefinedFlag = GetFirstUndefinedFlag(enumType);

            if (firstUndefinedFlag is not null)
            {
                yield return [firstUndefinedFlag];
                yield break;
            }
        }

        object firstInvalid = Enum.ToObject(enumType, -1);
        yield return [firstInvalid];

        long maxDefinedValue = Enum.GetValues(enumType)
            .Cast<object>()
            .Select(Convert.ToInt64)
            .DefaultIfEmpty()
            .Max();

        yield return [Enum.ToObject(enumType, maxDefinedValue + 1)];
    }

    private static object? GetFirstUndefinedFlag(Type enumType)
    {
        HashSet<ulong> definedValues = Enum.GetValues(enumType)
            .Cast<object>()
            .Select(Convert.ToUInt64)
            .ToHashSet();

        int bitCount = GetBitCount(Enum.GetUnderlyingType(enumType));

        for (int bit = 0; bit < bitCount; bit++)
        {
            ulong candidate = 1UL << bit;

            if (!definedValues.Contains(candidate))
            {
                return Enum.ToObject(enumType, candidate);
            }
        }

        return null;
    }

    private static int GetBitCount(Type underlyingType) => underlyingType.Name switch
    {
        nameof(Byte) or nameof(SByte) => 8,
        nameof(Int16) or nameof(UInt16) => 16,
        nameof(Int32) or nameof(UInt32) => 32,
        nameof(Int64) or nameof(UInt64) => 64,
        _ => throw new ArgumentOutOfRangeException(nameof(underlyingType))
    };
}
