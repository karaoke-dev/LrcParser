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

    [Test]
    public void TestDecodeWithRuby()
    {
        const string lrc_text = "[00:01:00]島[00:02:00]\n[00:03:00]島[00:04:00]\n[00:05:00]島[00:06:00]\n"
            + "@Ruby1=島,しま,,[00:02:00]\n@Ruby2=島,じま,[00:03:00],[00:04:00]\n@Ruby3=島,とう,[00:05:00]";

        var expected = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 1000 },
                        { new TextIndex(0, IndexState.End), 2000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "しま",
                            StartIndex = new TextIndex(0),
                            EndIndex = new TextIndex(0, IndexState.End)
                        }
                    }
                },
                new()
                {
                    Text = "島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 3000 },
                        { new TextIndex(0, IndexState.End), 4000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "じま",
                            StartIndex = new TextIndex(0),
                            EndIndex = new TextIndex(0, IndexState.End)
                        }
                    }
                },
                new()
                {
                    Text = "島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 5000 },
                        { new TextIndex(0, IndexState.End), 6000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "とう",
                            StartIndex = new TextIndex(0),
                            EndIndex = new TextIndex(0, IndexState.End)
                        }
                    }
                }
            }
        };

        var actual = Decode(lrc_text);
        areEqual(expected, actual);
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\n")]
    [TestCase(" \n ")]
    [TestCase(null)]
    public void TestDecodeWithEmptyFile(string lrcText)
    {
        var expected = new Song();
        var actual = Decode(lrcText);
        areEqual(expected, actual);
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

    [Test]
    public void TestEncodeWithRuby()
    {
        const string expected = "[00:01.00]島[00:02.00]\n[00:03.00]島[00:04.00]\n[00:05.00]島[00:06.00]\n\n"
                                + "@Ruby1=島,しま,,[00:02.00]\n@Ruby2=島,じま,[00:03.00],[00:04.00]\n@Ruby3=島,とう,[00:05.00]";

        var song = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 1000 },
                        { new TextIndex(0, IndexState.End), 2000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "しま",
                            StartIndex = new TextIndex(0),
                            EndIndex = new TextIndex(0, IndexState.End)
                        }
                    }
                },
                new()
                {
                    Text = "島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 3000 },
                        { new TextIndex(0, IndexState.End), 4000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "じま",
                            StartIndex = new TextIndex(0),
                            EndIndex = new TextIndex(0, IndexState.End)
                        }
                    }
                },
                new()
                {
                    Text = "島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 5000 },
                        { new TextIndex(0, IndexState.End), 6000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "とう",
                            StartIndex = new TextIndex(0),
                            EndIndex = new TextIndex(0, IndexState.End)
                        }
                    }
                }
            }
        };

        var actual = Encode(song);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestEncodeWithEmptyFile()
    {
        const string expected = "";

        var song = new Song();
        var actual = Encode(song);
        Assert.AreEqual(expected, actual);
    }

    private static void areEqual(Song expected, Song actual)
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
        Assert.AreEqual(expected.StartIndex, actual.StartIndex);
        Assert.AreEqual(expected.EndIndex, actual.EndIndex);
    }
}