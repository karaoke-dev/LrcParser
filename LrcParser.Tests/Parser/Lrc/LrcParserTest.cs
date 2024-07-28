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
            "[00:17.00] <00:00.00>帰<00:01.00>り<00:02.00>道<00:03.00>は<00:04.00>",
        };

        var song = new Song
        {
            Lyrics =
            [
                new Lyric
                {
                    Text = "帰り道は",
                    StartTime = 17000,
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 17000 },
                        { new TextIndex(1), 18000 },
                        { new TextIndex(2), 19000 },
                        { new TextIndex(3), 20000 },
                        { new TextIndex(3, IndexState.End), 21000 },
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
                    StartTime = 17000,
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 17000 },
                        { new TextIndex(1), 18000 },
                        { new TextIndex(2), 19000 },
                        { new TextIndex(3), 20000 },
                        { new TextIndex(3, IndexState.End), 21000 },
                    },
                },
            ],
        };

        var lrcText = new[]
        {
            "[00:17.00] <00:00.00>帰<00:01.00>り<00:02.00>道<00:03.00>は<00:04.00>",
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
