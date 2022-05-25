// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LrcParser.Extension;
using LrcParser.Model;

namespace LrcParser.Tests.Helper;

public static class TestCaseTagHelper
{
    /// <summary>
    /// Process test case time tag string format into <see cref="Tuple"/>
    /// </summary>
    /// <example>
    /// [0,start]:1000
    /// </example>
    /// <param name="str">Time tag string format</param>
    /// <returns><see cref="Tuple"/>Time tag object</returns>
    public static Tuple<TextIndex, int?> ParseTimeTag(string str)
    {
        if (string.IsNullOrEmpty(str))
            return new Tuple<TextIndex, int?>(new TextIndex(0), null);

        var regex = new Regex("(?<index>[-0-9]+),(?<state>start|end)]:(?<time>[-0-9]+|s*|)");
        var result = regex.Match(str);
        if (!result.Success)
            throw new RegexMatchTimeoutException(nameof(str));

        int index = result.GetGroupValue<int>("index");
        var state = result.GetGroupValue<string>("state") == "start" ? IndexState.Start : IndexState.End;
        int? time = result.GetGroupValue<int?>("time");

        return new Tuple<TextIndex, int?>(new TextIndex(index, state), time);
    }

    public static SortedDictionary<TextIndex, int?> ParseTimeTagsWithNullableTime(IEnumerable<string> strings)
    {
        var dictionary = strings.Select(ParseTimeTag).ToDictionary(k => k.Item1, v => v.Item2);

        return new SortedDictionary<TextIndex, int?>(dictionary);
    }

    public static SortedDictionary<TextIndex, int> ParseTimeTags(IEnumerable<string> strings)
    {
        var dictionary = ParseTimeTagsWithNullableTime(strings)
            .ToDictionary(k => k.Key, v =>
            {
                if (v.Value == null)
                    throw new ArgumentNullException(nameof(v.Value));

                return v.Value.Value;
            });

        return new SortedDictionary<TextIndex, int>(dictionary);
    }
}
