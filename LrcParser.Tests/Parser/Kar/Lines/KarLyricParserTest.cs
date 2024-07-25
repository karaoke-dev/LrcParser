// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using LrcParser.Parser.Kar.Lines;
using LrcParser.Parser.Kar.Metadata;
using LrcParser.Tests.Helper;
using LrcParser.Tests.Parser.Lines;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Kar.Lines;

public class KarLyricParserTest : BaseSingleLineParserTest<KarLyricParser, KarLyric>
{
    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", true)]
    [TestCase("karaoke", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    [TestCase("@Ruby1=帰,かえ", true)] // will take off this if no other parser to process this line.
    public void TestCanDecode(string text, bool expected)
    {
        var actual = CanDecode(text);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(testDecodeSource))]
    public void TestDecode(string lyric, KarLyric expected)
    {
        var actual = Decode(lyric);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static IEnumerable<object[]> testDecodeSource => new object[][]
    {
        [
            "[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]",
            new KarLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220"]),
            },
        ],
        [
            "帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]",
            new KarLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220"]),
            },
        ],
        [
            "[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は",
            new KarLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940"]),
            },
        ],
        [
            "帰り道は",
            new KarLyric
            {
                Text = "帰り道は",
                TimeTags = [],
            },
        ],
        [
            "",
            new KarLyric
            {
                Text = "",
                TimeTags = [],
            },
        ],
        [
            null!,
            new KarLyric
            {
                Text = "",
                TimeTags = [],
            },
        ],
    };

    [TestCaseSource(nameof(testEncodeSource))]
    public void TestEncode(KarLyric lyric, string expected)
    {
        var actual = Encode(lyric);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static IEnumerable<object[]> testEncodeSource => new object[][]
    {
        [
            new KarLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220"]),
            },
            "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]",
        ],
        [
            new KarLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220"]),
            },
            "帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]",
        ],
        [
            new KarLyric
            {
                Text = "帰り道は",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940"]),
            },
            "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は",
        ],
        [
            new KarLyric
            {
                Text = "帰り道は",
                TimeTags = [],
            },
            "帰り道は",
        ],
        [
            new KarLyric
            {
                Text = "",
                TimeTags = [],
            },
            "",
        ],
    };
}
