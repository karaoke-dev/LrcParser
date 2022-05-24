// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Utils;

internal static class TypeUtils
{
    internal static TType? ChangeType<TType>(object? value)
    {
        if (value == null)
            return default;

        var type = typeof(TType);
        return (TType)Convert.ChangeType(value, Nullable.GetUnderlyingType(type) ?? type);
    }
}
