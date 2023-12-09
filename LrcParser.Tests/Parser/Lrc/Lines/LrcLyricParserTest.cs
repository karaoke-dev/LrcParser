// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

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
    [TestCase("@Ruby1=帰,かえ", true)] // will take off this if no other parser to process this line.
    public void TestCanDecode(string text, bool expected)
    {
        var actual = CanDecode(text);
        Assert.AreEqual(expected, actual);
    }

    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", "帰り道は", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" })]
    [TestCase("帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", "帰り道は", new[] { "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" })]
    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は", "帰り道は", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940" })]
    [TestCase("帰り道は", "帰り道は", new string[] { })]
    [TestCase("", "", new string[] { })]
    [TestCase(null, "", new string[] { })]
    public void TestDecode(string lyric, string text, string[] timeTags)
    {
        var expected = new LrcLyric
        {
            Text = text,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };
        var actual = Decode(lyric);

        Assert.AreEqual(expected.Text, actual.Text);
        Assert.AreEqual(expected.TimeTags, actual.TimeTags);
    }

    [TestCase("帰り道は", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" }, "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]")]
    [TestCase("帰り道は", new[] { "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" }, "帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]")]
    [TestCase("帰り道は", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940" }, "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は")]
    [TestCase("帰り道は", new string[] { }, "帰り道は")]
    [TestCase("", new string[] { }, "")]
    public void TestEncode(string text, string[] timeTags, string expected)
    {
        var lyric = new LrcLyric
        {
            Text = text,
            TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags),
        };
        var actual = Encode(lyric);

        Assert.AreEqual(expected, actual);
    }
}
