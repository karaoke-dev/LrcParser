// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using LrcParser.Utils;

namespace LrcParser.Extension;

internal static class RegexExtensions
{
    internal static TType? GetGroupValue<TType>(this Match match, string key, bool useDefaultValueIfEmpty = true)
    {
        string? value = match.Groups[key]?.Value;

        // if got empty value, should change to null.
        return TypeUtils.ChangeType<TType>(string.IsNullOrEmpty(value) ? null : value);
    }
}
