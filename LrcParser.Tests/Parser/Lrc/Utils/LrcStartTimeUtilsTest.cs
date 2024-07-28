// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using LrcParser.Parser.Lrc.Utils;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Lrc.Utils;

public class LrcStartTimeUtilsTest
{
    #region Decode

    [TestCase("[1:00.00] ", new[] { 60000 }, "")]
    [TestCase("[1:00.00][1:02.00] Lyric", new[] { 60000, 62000 }, "Lyric")]
    [TestCase("[1:00.00]Lyric", new[] { 60000 }, "Lyric")] // With no spacing.
    [TestCase("[1:00.00]   Lyric", new[] { 60000 }, "Lyric")] // With lots of spacing.
    [TestCase("[1:00.00] <00:00.04> Lyric <00:00.16>", new[] { 60000 }, "<00:00.04> Lyric <00:00.16>")] // With time-tag.
    [TestCase("[1:00.00] <00:00.04> Lyric", new[] { 60000 }, "<00:00.04> Lyric")] // With time-tag.
    [TestCase("[1:00.00] <00:00.04> Lyric  ", new[] { 60000 }, "<00:00.04> Lyric")] // Remove the end spacing.
    public void TestDecodeWithValidLine(string line, int[] expectedStartTimes, string lyric)
    {
        var actual = LrcStartTimeUtils.SplitLyricAndTimeTag(line);

        Assert.That(actual.Item1, Is.EqualTo(expectedStartTimes));
        Assert.That(actual.Item2, Is.EqualTo(lyric));
    }

    [TestCase("Lyric", new int[] { }, "Lyric")] // With no start time.
    [TestCase("   Lyric", new int[] { }, "Lyric")] // With no start time.
    [TestCase("<00:00.04> Lyric <00:00.16>", new int[] { }, "<00:00.04> Lyric <00:00.16>")] // With no start time but with time-tag.
    public void TestDecodeWithInvalidLine(string line, int[] expectedStartTimes, string lyric)
    {
        var actual = LrcStartTimeUtils.SplitLyricAndTimeTag(line);

        // still return the value, but let outside handle the invalid value.
        Assert.That(actual.Item1, Is.EqualTo(expectedStartTimes));
        Assert.That(actual.Item2, Is.EqualTo(lyric));
    }

    [TestCase("[00:00.00]", 0)]
    [TestCase("[00:06.00]", 6000)]
    [TestCase("[01:00.00]", 60000)]
    [TestCase("[10:00.00]", 600000)]
    [TestCase("[100:00.00]", 6000000)]
    [TestCase("[0:00.00]", 0)] // prevent throw error in some invalid format.
    [TestCase("[0:0.0]", 0)] // prevent throw error in some invalid format.
    [TestCase("[1:00.00][1:02.00]", 60000)] // rarely to get this case, so return the first one.
    public void TestConvertTimeTagToMillionSecond(string timeTag, int expectedMillionSecond)
    {
        var actual = LrcStartTimeUtils.ConvertTimeTagToMillionSecond(timeTag);

        Assert.That(actual, Is.EqualTo(expectedMillionSecond));
    }

    [TestCase("[--:--.--]")]
    [TestCase("[]")]
    [TestCase("<1:00.00>")] // should not contains embedded time-tag.
    public void TestConvertTimeTagToMillionSecondWithInvalidValue(string timeTag)
    {
        Assert.Throws<InvalidOperationException>(() => LrcStartTimeUtils.ConvertTimeTagToMillionSecond(timeTag));
    }

    #endregion

    #region Encode

    [TestCase(new[] { 60000 }, "Lyric", "[01:00.00] Lyric")]
    [TestCase(new[] { 60000, 62000 }, "Lyric", "[01:00.00][01:02.00] Lyric")]
    [TestCase(new[] { 60000 }, "<00:00.04> Lyric <00:00.16>", "[01:00.00] <00:00.04> Lyric <00:00.16>")] // With time-tag.
    [TestCase(new[] { 60000 }, "  Lyric", "[01:00.00] Lyric")] // Start spacing will be removed automatically.
    public void TestEncodeWithValidValue(int[] startTimes, string lyric, string expectedLine)
    {
        var actual = LrcStartTimeUtils.JoinLyricAndTimeTag(startTimes, lyric);

        Assert.That(actual, Is.EqualTo(expectedLine));
    }

    [TestCase(new int[] { }, "Lyric")] // With no start time.
    [TestCase(new int[] { }, "[00:00.00] Lyric")] // Lyric should not contains any start time-tag info.
    public void TestEncodeWithInvalidValue(int[] startTimes, string expectedLine)
    {
        Assert.Throws<InvalidOperationException>(() => LrcStartTimeUtils.JoinLyricAndTimeTag(startTimes, expectedLine));
    }

    [TestCase(0, "[00:00.00]")]
    [TestCase(6000, "[00:06.00]")]
    [TestCase(60000, "[01:00.00]")]
    [TestCase(600000, "[10:00.00]")]
    [TestCase(6000000, "[100:00.00]")]
    public void TestConvertMillionSecondToTimeTag(int millionSecond, string expectedTimeTag)
    {
        var actual = LrcStartTimeUtils.ConvertMillionSecondToTimeTag(millionSecond);

        Assert.That(actual, Is.EqualTo(expectedTimeTag));
    }

    [TestCase(-1)]
    public void TestConvertMillionSecondToTimeTagWithInvalidValue(int millionSecond)
    {
        Assert.Throws<InvalidOperationException>(() => LrcStartTimeUtils.ConvertMillionSecondToTimeTag(millionSecond));
    }

    #endregion
}
