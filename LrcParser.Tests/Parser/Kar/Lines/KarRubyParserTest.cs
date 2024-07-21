// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

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

    [TestCase("@Ruby1=帰,かえ,[00:53:19],[01:24:77]", "帰", "かえ", new string[] { }, 53190, 84770)]
    [TestCase("@Ruby1=帰,かえ,[01:24:77]", "帰", "かえ", new string[] { }, 84770, null)]
    [TestCase("@Ruby1=帰,かえ,,[01:24:77]", "帰", "かえ", new string[] { }, null, 84770)]
    [TestCase("@Ruby1=帰,かえ", "帰", "かえ", new string[] { }, null, null)]
    [TestCase("@Ruby1=帰,か[00:00:50]え", "帰", "かえ", new[] { "[1,start]:500" }, null, null)]
    public void TestDecode(string rubyTag, string parent, string ruby, string[] timeTags, int? startTime, int? endTime)
    {
        var expected = new KarRuby
        {
            Parent = parent,
            Ruby = ruby,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            StartTime = startTime,
            EndTime = endTime,
        };
        var actual = Decode(rubyTag);

        Assert.That(actual.Ruby, Is.EqualTo(expected.Ruby));
        Assert.That(actual.Parent, Is.EqualTo(expected.Parent));
        Assert.That(actual.TimeTags, Is.EqualTo(expected.TimeTags));
        Assert.That(actual.StartTime, Is.EqualTo(expected.StartTime));
        Assert.That(actual.EndTime, Is.EqualTo(expected.EndTime));
    }

    [TestCase("帰", "かえ", new string[] { }, 53190, 84770, "@Ruby1=帰,かえ,[00:53.19],[01:24.77]")]
    [TestCase("帰", "かえ", new string[] { }, 84770, null, "@Ruby1=帰,かえ,[01:24.77]")]
    [TestCase("帰", "かえ", new string[] { }, null, 84770, "@Ruby1=帰,かえ,,[01:24.77]")]
    [TestCase("帰", "かえ", new string[] { }, null, null, "@Ruby1=帰,かえ")]
    [TestCase("帰", "かえ", new[] { "[1,start]:500" }, null, null, "@Ruby1=帰,か[00:00.50]え")]
    public void TestEncode(string parent, string ruby, string[] timeTags, int? startTime, int? endTime, string expected)
    {
        var rubyTag = new KarRuby
        {
            Parent = parent,
            Ruby = ruby,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            StartTime = startTime,
            EndTime = endTime,
        };
        var actual = Encode(rubyTag);

        Assert.That(actual, Is.EqualTo(expected));
    }
}
