// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using LrcParser.Parser.Lrc.Lines;
using LrcParser.Parser.Lrc.Metadata;
using LrcParser.Tests.Helper;
using LrcParser.Tests.Parser.Lines;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Lrc.Lines;

public class LrcLyricParserTest : BaseSingleLineParserTest<LrcLyricParser, LrcLyric>
{
    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", true)]
    [TestCase("karaoke", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void TestCanDecode(string text, bool expected)
    {
        var actual = CanDecode(text);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(testDecodeSource))]
    public void TestDecode(string lyric, LrcLyric expected)
    {
        var actual = Decode(lyric);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static IEnumerable<object[]> testDecodeSource => new object[][]
    {
        [
            "[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]",
            new LrcLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220"]),
            },
        ],
        [
            "帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]",
            new LrcLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220"]),
            },
        ],
        [
            "[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は",
            new LrcLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940"]),
            },
        ],
        [
            "帰り道は",
            new LrcLyric
            {
                Text = "帰り道は",
                TimeTags = [],
            },
        ],
        [
            "",
            new LrcLyric
            {
                Text = "",
                TimeTags = [],
            },
        ],
        [
            null!,
            new LrcLyric
            {
                Text = "",
                TimeTags = [],
            },
        ],
    };

    [TestCaseSource(nameof(testEncodeSource))]
    public void TestEncode(LrcLyric lyric, string expected)
    {
        var actual = Encode(lyric);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static IEnumerable<object[]> testEncodeSource => new object[][]
    {
        [
            new LrcLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220"]),
            },
            "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]",
        ],
        [
            new LrcLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220"]),
            },
            "帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]",
        ],
        [
            new LrcLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940"]),
            },
            "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は",
        ],
        [
            new LrcLyric
            {
                Text = "帰り道は",
                TimeTags = [],
            },
            "帰り道は",
        ],
        [
            new LrcLyric
            {
                Text = "",
                TimeTags = [],
            },
            "",
        ],
    };
}
