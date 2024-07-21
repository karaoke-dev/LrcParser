// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using LrcParser.Model;
using NUnit.Framework;

namespace LrcParser.Tests.Parser.Lrc;

public class LrcParserTest : BaseLyricParserTest<LrcParser.Parser.Lrc.LrcParser>
{
    [Test]
    public void TestDecode()
    {
        var lrcText = new[]
        {
            "[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]",
        };

        var song = new Song
        {
            Lyrics =
            [
                new Lyric
                {
                    Text = "帰り道は",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 17970 },
                        { new TextIndex(1), 18370 },
                        { new TextIndex(2), 18550 },
                        { new TextIndex(3), 18940 },
                        { new TextIndex(3, IndexState.End), 19220 },
                    },
                },
            ],
        };

        checkDecode(lrcText, song);
    }

    [Test]
    public void TestEncode()
    {
        var song = new Song
        {
            Lyrics =
            [
                new Lyric
                {
                    Text = "帰り道は",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 17970 },
                        { new TextIndex(1), 18370 },
                        { new TextIndex(2), 18550 },
                        { new TextIndex(3), 18940 },
                        { new TextIndex(3, IndexState.End), 19220 },
                    },
                },
            ],
        };

        var lrcText = new[]
        {
            "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]",
        };

        checkEncode(song, lrcText);
    }

    [Test]
    public void TestEncodeWithEmptyFile()
    {
        var song = new Song();

        var lrcText = new[]
        {
            "",
        };

        checkEncode(song, lrcText);
    }

    private void checkDecode(string[] lrcTexts, Song song)
    {
        var actual = Decode(string.Join('\n', lrcTexts));
        AreEqual(song, actual);
    }

    private void checkEncode(Song song, string[] lrcTexts)
    {
        var expected = string.Join('\n', lrcTexts);
        var actual = Encode(song);
        Assert.That(actual, Is.EqualTo(expected));
    }
}
