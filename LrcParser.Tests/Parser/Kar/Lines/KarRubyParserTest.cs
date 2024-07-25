// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using LrcParser.Parser.Kar.Lines;
using LrcParser.Parser.Kar.Metadata;
using LrcParser.Tests.Helper;
using LrcParser.Tests.Parser.Lines;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Kar.Lines;

public class KarRubyParserTest : BaseSingleLineParserTest<KarRubyParser, KarRuby>
{
    [TestCase("@Ruby1=帰,かえ", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", false)]
    [TestCase("karaoke", false)]
    public void TestCanDecode(string text, bool expected)
    {
        var actual = CanDecode(text);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(testDecodeSource))]
    public void TestDecode(string rubyTag, KarRuby expected)
    {
        var actual = Decode(rubyTag);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static IEnumerable<object[]> testDecodeSource => new object[][]
    {
        [
            "@Ruby1=帰,かえ,[00:53:19],[01:24:77]",
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(Array.Empty<string>()),
                StartTime = 53190,
                EndTime = 84770,
            },
        ],
        [
            "@Ruby1=帰,かえ,[01:24:77]",
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(Array.Empty<string>()),
                StartTime = 84770,
                EndTime = null,
            },
        ],
        [
            "@Ruby1=帰,かえ,,[01:24:77]",
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(Array.Empty<string>()),
                StartTime = null,
                EndTime = 84770,
            },
        ],
        [
            "@Ruby1=帰,かえ",
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(Array.Empty<string>()),
                StartTime = null,
                EndTime = null,
            },
        ],
        [
            "@Ruby1=帰,か[00:00:50]え",
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[1,start]:500" }),
                StartTime = null,
                EndTime = null,
            },
        ],
    };

    [TestCaseSource(nameof(testEncodeSources))]
    public void TestEncode(KarRuby rubyTag, string expected)
    {
        var actual = Encode(rubyTag);

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static IEnumerable<object[]> testEncodeSources => new object[][]
    {
        [
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = [],
                StartTime = 53190,
                EndTime = 84770,
            },
            "@Ruby1=帰,かえ,[00:53.19],[01:24.77]",
        ],
        [
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = [],
                StartTime = 84770,
                EndTime = null,
            },
            "@Ruby1=帰,かえ,[01:24.77]",
        ],
        [
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = [],
                StartTime = null,
                EndTime = 84770,
            },
            "@Ruby1=帰,かえ,,[01:24.77]",
        ],
        [
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = [],
                StartTime = null,
                EndTime = null,
            },
            "@Ruby1=帰,かえ",
        ],
        [
            new KarRuby
            {
                Parent = "帰",
                Ruby = "かえ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(["[1,start]:500"]),
                StartTime = null,
                EndTime = null,
            },
            "@Ruby1=帰,か[00:00.50]え",
        ],
    };
}
