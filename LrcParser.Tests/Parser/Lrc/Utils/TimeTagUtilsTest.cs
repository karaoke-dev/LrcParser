// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Parser.Lrc.Utils;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Lrc.Utils;

public class TimeTagUtilsTest
{
    [TestCase("[00:01:00]", 1000)]
    public void TestTimeTagToMillionSecond(string timeTag, int millionSecond)
    {
        var actual = TimeTagUtils.TimeTagToMillionSecond(timeTag);

        Assert.AreEqual(millionSecond, actual);
    }

    [TestCase(1000, "[00:01.00]")]
    public void TestTimeTagToMillionSecond(int millionSecond, string timeTag)
    {
        var actual = TimeTagUtils.MillionSecondToTimeTag(millionSecond);

        Assert.AreEqual(timeTag, actual);
    }
}
