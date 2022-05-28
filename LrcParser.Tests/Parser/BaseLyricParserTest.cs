// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using LrcParser.Model;
using LrcParser.Parser;
using NUnit.Framework;

namespace LrcParser.Tests.Parser;

public class BaseLyricParserTest<TParser> where TParser : LyricParser, new()
{
    protected Song Decode(string text) => new TParser().Decode(text);

    protected string Encode(Song component) => new TParser().Encode(component);

    protected static void AreEqual(Song expected, Song actual)
    {
        var expectedLyrics = expected.Lyrics;
        var actualLyrics = actual.Lyrics;
        var index = Math.Max(expectedLyrics.Count, actualLyrics.Count);
        for (int i = 0; i < index; i++)
        {
            areEqual(expectedLyrics[i], actualLyrics[i]);
        }
    }

    private static void areEqual(Lyric expected, Lyric actual)
    {
        Assert.AreEqual(expected.Text, actual.Text);
        Assert.AreEqual(expected.TimeTags, actual.TimeTags);

        var expectedRubies = expected.RubyTags;
        var actualRubies = actual.RubyTags;
        var index = Math.Max(expectedRubies.Count, actualRubies.Count);
        for (int i = 0; i < index; i++)
        {
            areEqual(expectedRubies[i], actualRubies[i]);
        }
    }

    private static void areEqual(RubyTag expected, RubyTag actual)
    {
        Assert.AreEqual(expected.Text, actual.Text);
        Assert.AreEqual(expected.TimeTags, actual.TimeTags);
        Assert.AreEqual(expected.StartIndex, actual.StartIndex);
        Assert.AreEqual(expected.EndIndex, actual.EndIndex);
    }
}
