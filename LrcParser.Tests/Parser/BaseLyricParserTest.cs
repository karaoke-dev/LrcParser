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
        Assert.That(actual.Text, Is.EqualTo(expected.Text));
        Assert.That(actual.TimeTags, Is.EqualTo(expected.TimeTags));

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
        Assert.That(actual.Text, Is.EqualTo(expected.Text));
        Assert.That(actual.TimeTags, Is.EqualTo(expected.TimeTags));
        Assert.That(actual.StartCharIndex, Is.EqualTo(expected.StartCharIndex));
        Assert.That(actual.EndCharIndex, Is.EqualTo(expected.EndCharIndex));
    }
}
