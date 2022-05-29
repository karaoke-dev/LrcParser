// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using LrcParser.Parser.Lrc.Utils;
using LrcParser.Tests.Helper;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Lrc.Utils;

public class LrcTimedTextUtilsTest
{
    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", "帰り道は", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" })]
    [TestCase(" [00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", " 帰り道は", new[] { "[1,start]:17970", "[2,start]:18370", "[3,start]:18550", "[4,start]:18940", "[4,end]:19220" })]
    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22] ", "帰り道は ", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" })]
    [TestCase("帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]", "帰り道は", new[] { "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" })]
    [TestCase("[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は", "帰り道は", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940" })]
    [TestCase("帰り道は", "帰り道は", new string[] { })]
    [TestCase("", "", new string[] { })]
    [TestCase(null, "", new string[] { })]
    public void TestDecode(string text, string expectedText, string[] expectedTimeTags)
    {
        var (actualText, actualTimeTags) = LrcTimedTextUtils.TimedTextToObject(text);

        Assert.AreEqual(expectedText, actualText);
        Assert.AreEqual(TestCaseTagHelper.ParseTimeTags(expectedTimeTags), actualTimeTags);
    }

    [TestCase("帰り道は", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" }, "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]")]
    [TestCase(" 帰り道は", new[] { "[1,start]:17970", "[2,start]:18370", "[3,start]:18550", "[4,start]:18940", "[4,end]:19220" }, " [00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]")]
    [TestCase("帰り道は ", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" }, "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22] ")]
    [TestCase("帰り道は", new[] { "[1,start]:18370", "[2,start]:18550", "[3,start]:18940", "[3,end]:19220" }, "帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]")]
    [TestCase("帰り道は", new[] { "[0,start]:17970", "[1,start]:18370", "[2,start]:18550", "[3,start]:18940" }, "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は")]
    [TestCase("帰り道は", new string[] { }, "帰り道は")]
    [TestCase("", new string[] { }, "")]
    public void TestEncode(string text, string[] timeTags, string expected)
    {
        var actual = LrcTimedTextUtils.ToTimedText(text, TestCaseTagHelper.ParseTimeTags(timeTags));

        Assert.AreEqual(expected, actual);
    }
}
