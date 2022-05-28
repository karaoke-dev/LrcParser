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
        AreEqual(expected, actual);
    }

    [Test]
    public void TestDecodeWithRuby()
    {
        const string lrc_text = "[00:01:00]島[00:02:00]島[00:03:00]島[00:04:00]\n"
            + "@Ruby1=島,しま,,[00:02:00]\n@Ruby2=島,じま,[00:02:00],[00:03:00]\n@Ruby3=島,とう,[00:03:00]";

        var expected = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "島島島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 1000 },
                        { new TextIndex(1), 2000 },
                        { new TextIndex(2), 3000 },
                        { new TextIndex(2, IndexState.End), 4000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "しま",
                            StartIndex = 0,
                            EndIndex = 1
                        },
                        new()
                        {
                            Text = "じま",
                            StartIndex = 1,
                            EndIndex = 2
                        },
                        new()
                        {
                            Text = "とう",
                            StartIndex = 2,
                            EndIndex = 3
                        }
                    }
                },
            }
        };

        var actual = Decode(lrc_text);
        AreEqual(expected, actual);
    }

    [Test]
    public void TestDecodeWithNoTimeRangeRuby()
    {
        const string lrc_text = "カラオケ\n@Ruby1=カ,か\n@Ruby2=ラ,ら\n@Ruby3=オ,お\n@Ruby4=ケ,け";

        var expected = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "カラオケ",
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "か",
                            StartIndex = 0,
                            EndIndex = 1
                        },
                        new()
                        {
                            Text = "ら",
                            StartIndex = 1,
                            EndIndex = 2
                        },
                        new()
                        {
                            Text = "お",
                            StartIndex = 2,
                            EndIndex = 3
                        },
                        new()
                        {
                            Text = "け",
                            StartIndex = 3,
                            EndIndex = 4
                        }
                    }
                },
            }
        };

        var actual = Decode(lrc_text);
        AreEqual(expected, actual);
    }

    [Test]
    public void TestDecodeWithRubyInDifferentLine()
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
                            StartIndex = 0,
                            EndIndex = 1
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
                            StartIndex = 0,
                            EndIndex = 1
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
                            StartIndex = 0,
                            EndIndex = 1
                        }
                    }
                }
            }
        };

        var actual = Decode(lrc_text);
        AreEqual(expected, actual);
    }

    [Test]
    public void TestDecodeWithInvalid()
    {
        // should not generate the ruby if ruby text is same as parent text.
        const string lrc_text = "[00:01:00]島[00:02:00]\n@Ruby1=島,島";

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
                },
            }
        };

        var actual = Decode(lrc_text);
        AreEqual(expected, actual);
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
        const string expected = "[00:01.00]島[00:02.00]島[00:03.00]島[00:04.00]\n\n"
                                + "@Ruby1=島,しま,,[00:02.00]\n@Ruby2=島,じま,[00:02.00],[00:03.00]\n@Ruby3=島,とう,[00:03.00]";

        var song = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "島島島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 1000 },
                        { new TextIndex(1), 2000 },
                        { new TextIndex(2), 3000 },
                        { new TextIndex(2, IndexState.End), 4000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "しま",
                            StartIndex = 0,
                            EndIndex = 1
                        },
                        new()
                        {
                            Text = "じま",
                            StartIndex = 1,
                            EndIndex = 2
                        },
                        new()
                        {
                            Text = "とう",
                            StartIndex = 2,
                            EndIndex = 3
                        }
                    }
                },
            }
        };

        var actual = Encode(song);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestEncodeWithNoTimeRangeRuby()
    {
        const string expected = "カラオケ\n\n@Ruby1=カ,か\n@Ruby2=ラ,ら\n@Ruby3=オ,お\n@Ruby4=ケ,け";

        var song = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "カラオケ",
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "か",
                            StartIndex = 0,
                            EndIndex = 1
                        },
                        new()
                        {
                            Text = "ら",
                            StartIndex = 1,
                            EndIndex = 2
                        },
                        new()
                        {
                            Text = "お",
                            StartIndex = 2,
                            EndIndex = 3
                        },
                        new()
                        {
                            Text = "け",
                            StartIndex = 3,
                            EndIndex = 4
                        }
                    }
                },
            }
        };

        var actual = Encode(song);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void TestEncodeWithRubyInDifferentLine()
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
                            StartIndex = 0,
                            EndIndex = 1
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
                            StartIndex = 0,
                            EndIndex = 1
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
                            StartIndex = 0,
                            EndIndex = 1
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
}
