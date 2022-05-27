// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Parser.Lrc.Lines;
using LrcParser.Parser.Lrc.Metadata;
using LrcParser.Tests.Helper;
using LrcParser.Tests.Parser.Lines;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Lrc.Lines;

public class LrcRubyParserTest : BaseSingleLineParserTest<LrcRubyParser, LrcRuby>
{
    [TestCase("@Ruby1=帰,かえ", true)]
    [TestCase("", false)]
    [TestCase(null, false)]
    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", false)]
    [TestCase("karaoke", false)]
    public void TestCanDecode(string text, bool expected)
    {
        var actual = CanDecode(text);
        Assert.AreEqual(expected, actual);
    }

    [TestCase("@Ruby1=帰,かえ,[00:53:19],[01:24:77]", "帰", "かえ", new string[] { }, 53190, 84770)]
    [TestCase("@Ruby1=帰,かえ,[01:24:77]", "帰", "かえ", new string[] { },84770, null)]
    [TestCase("@Ruby1=帰,かえ,,[01:24:77]", "帰", "かえ", new string[] { },null, 84770)]
    [TestCase("@Ruby1=帰,かえ", "帰", "かえ", new string[] { },null, null)]
    [TestCase("@Ruby1=帰,か[01:24:77]え", "帰", "かえ", new[] { "[1,start]:84770" },null, null)]
    public void TestDecode(string rubyTag, string parent, string ruby, string[] timeTags, int? startTime, int? endTime)
    {
        var expected = new LrcRuby
        {
            Parent = parent,
            Ruby = ruby,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            StartTime = startTime,
            EndTime = endTime
        };
        var actual = Decode(rubyTag);

        Assert.AreEqual(expected.Ruby, actual.Ruby);
        Assert.AreEqual(expected.Parent, actual.Parent);
        Assert.AreEqual(expected.StartTime, actual.StartTime);
        Assert.AreEqual(expected.EndTime, actual.EndTime);
    }


    [TestCase("帰", "かえ", new string[] { }, 53190, 84770, "@Ruby1=帰,かえ,[00:53.19],[01:24.77]")]
    [TestCase("帰", "かえ", new string[] { }, 84770, null, "@Ruby1=帰,かえ,[01:24.77]")]
    [TestCase("帰", "かえ", new string[] { }, null, 84770, "@Ruby1=帰,かえ,,[01:24.77]")]
    [TestCase("帰", "かえ", new string[] { }, null, null, "@Ruby1=帰,かえ")]
    [TestCase("帰", "かえ", new[] { "[1,start]:84770" }, null, null, "@Ruby1=帰,か[01:24.77]え")]
    public void TestEncode(string parent, string ruby, string[] timeTags, int? startTime, int? endTime, string expected)
    {
        var rubyTag = new LrcRuby
        {
            Parent = parent,
            Ruby = ruby,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
            StartTime = startTime,
            EndTime = endTime
        };
        var actual = Encode(rubyTag);

        Assert.AreEqual(expected, actual);
    }
}
