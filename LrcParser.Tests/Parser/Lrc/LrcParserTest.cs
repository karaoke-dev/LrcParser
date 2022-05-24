// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using LrcParser.Model;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Lrc;

public class LrcParserTest : BaseLyricParserTest<LrcParser.Parser.Lrc.LrcParser>
{
    [Test]
    public void TestDecode()
    {
        const string lrc_text = "[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]";

        var expected = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "帰り道は",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 17970 },
                        { new TextIndex(1), 18370 },
                        { new TextIndex(2), 18550 },
                        { new TextIndex(3), 18940 },
                        { new TextIndex(3, IndexState.End), 19220 },
                    }
                }
            }
        };
        var actual = Decode(lrc_text);
        areEqual(expected, actual);
    }

    [Ignore("Waiting for implementation")]
    [Test]
    public void TestDecodeWithRuby()
    {

    }

    [Test]
    public void TestEncode()
    {
        const string expected = "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]";

        var song = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "帰り道は",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 17970 },
                        { new TextIndex(1), 18370 },
                        { new TextIndex(2), 18550 },
                        { new TextIndex(3), 18940 },
                        { new TextIndex(3, IndexState.End), 19220 },
                    }
                }
            }
        };
        var actual = Encode(song);
        Assert.AreEqual(expected, actual);
    }

    [Ignore("Waiting for implementation")]
    [Test]
    public void TestEncodeWithRuby()
    {

    }

    private void areEqual(Song expected, Song actual)
    {
        var expectedLyrics = expected.Lyrics;
        var actualLyrics = actual.Lyrics;
        var index = Math.Max(expectedLyrics.Count, actualLyrics.Count);
        for (int i = 0; i < index; i++)
        {
            areEqual(expectedLyrics[i], actualLyrics[i]);
        }
    }

    private void areEqual(Lyric expected, Lyric actual)
    {
        Assert.AreEqual(expected.Text, actual.Text);
        Assert.AreEqual(expected.TimeTags, actual.TimeTags);
    }
}
