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
            "[00:17:97]帰[00:18:37]り[00:18:55]道[00:18:94]は[00:19:22]"
        };

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

        checkDecode(lrcText, song);
    }

    [Test]
    public void TestDecodeWithRuby()
    {
        var lrcText = new[]
        {
            "[00:01:00]島[00:02:00]島[00:03:00]島[00:04:00]",
            "@Ruby1=島,しま,,[00:02:00]",
            "@Ruby2=島,じま,[00:02:00],[00:03:00]",
            "@Ruby3=島,とう,[00:03:00]"
        };

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

        checkDecode(lrcText, song);
    }

    [Test]
    public void TestDecodeWithRubyAndRubyTimeTag()
    {
        var lrcText = new[]
        {
            "[00:01:00]島[00:02:00]島[00:03:00]島[00:04:00]",
            "@Ruby1=島,し[00:00:50]ま,,[00:02:00]",
            "@Ruby2=島,じ[00:00:50]ま,[00:02:00],[00:03:00]",
            "@Ruby3=島,と[00:00:50]う,[00:03:00]"
        };

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
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 1500}
                            },
                            StartIndex = 0,
                            EndIndex = 1
                        },
                        new()
                        {
                            Text = "じま",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 2500}
                            },
                            StartIndex = 1,
                            EndIndex = 2
                        },
                        new()
                        {
                            Text = "とう",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 3500}
                            },
                            StartIndex = 2,
                            EndIndex = 3
                        }
                    }
                },
            }
        };

        checkDecode(lrcText, song);
    }

    [Test]
    public void TestDecodeWithSameRubyWithDifferentRubyTimeTag()
    {
        var lrcText = new[]
        {
            "[00:01:00]島[00:02:00]島[00:03:00]島[00:04:00]",
            "@Ruby1=島,し[00:00:40]ま,,[00:02:00]",
            "@Ruby2=島,し[00:00:50]ま,[00:02:00],[00:03:00]",
            "@Ruby3=島,し[00:00:60]ま,[00:03:00]"
        };

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
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 1400}
                            },
                            StartIndex = 0,
                            EndIndex = 1
                        },
                        new()
                        {
                            Text = "しま",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 2500}
                            },
                            StartIndex = 1,
                            EndIndex = 2
                        },
                        new()
                        {
                            Text = "しま",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 3600}
                            },
                            StartIndex = 2,
                            EndIndex = 3
                        }
                    }
                },
            }
        };

        checkDecode(lrcText, song);
    }

    [Test]
    public void TestDecodeWithNoTimeRangeRuby()
    {
        var lrcText = new[]
        {
            "カラオケ",
            "@Ruby1=カ,か",
            "@Ruby2=ラ,ら",
            "@Ruby3=オ,お",
            "@Ruby4=ケ,け"
        };

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

        checkDecode(lrcText, song);
    }

    [Test]
    public void TestDecodeWithRubyInDifferentLine()
    {
        var lrcText = new[]
        {
            "[00:01:00]島[00:02:00]",
            "[00:03:00]島[00:04:00]",
            "[00:05:00]島[00:06:00]",
            "@Ruby1=島,しま,,[00:02:00]",
            "@Ruby2=島,じま,[00:03:00],[00:04:00]",
            "@Ruby3=島,とう,[00:05:00]"
        };

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

        checkDecode(lrcText, song);
    }

    [Test]
    public void TestDecodeWithInvalid()
    {
        // should not generate the ruby if ruby text is same as parent text.
        var lrcText = new[]
        {
            "[00:01:00]島[00:02:00]",
            "@Ruby1=島,島"
        };

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
                },
            }
        };

        checkDecode(lrcText, song);
    }

    [Test]
    public void TestEncode()
    {
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

        var lrcText = new[]
        {
            "[00:17.97]帰[00:18.37]り[00:18.55]道[00:18.94]は[00:19.22]"
        };

        checkEncode(song, lrcText);
    }

    [Test]
    public void TestEncodeWithRuby()
    {
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

        var lrcText = new[]
        {
            "[00:01.00]島[00:02.00]島[00:03.00]島[00:04.00]",
            "",
            "@Ruby1=島,しま,,[00:02.00]",
            "@Ruby2=島,じま,[00:02.00],[00:03.00]",
            "@Ruby3=島,とう,[00:03.00]"
        };

        checkEncode(song, lrcText);
    }

    [Test]
    public void TestEncodeWithRubyAndRubyTimeTag()
    {
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
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 1500}
                            },
                            StartIndex = 0,
                            EndIndex = 1
                        },
                        new()
                        {
                            Text = "じま",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 2500}
                            },
                            StartIndex = 1,
                            EndIndex = 2
                        },
                        new()
                        {
                            Text = "とう",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 3500}
                            },
                            StartIndex = 2,
                            EndIndex = 3
                        }
                    }
                },
            }
        };

        var lrcText = new[]
        {
            "[00:01.00]島[00:02.00]島[00:03.00]島[00:04.00]",
            "",
            "@Ruby1=島,し[00:00.50]ま,,[00:02.00]",
            "@Ruby2=島,じ[00:00.50]ま,[00:02.00],[00:03.00]",
            "@Ruby3=島,と[00:00.50]う,[00:03.00]"
        };

        checkEncode(song, lrcText);
    }

    [Test]
    public void TestEncodeWithSameRubyWithDifferentRubyTimeTag()
    {
        var song = new Song
        {
            Lyrics = new List<Lyric>
            {
                new()
                {
                    Text = "島島島島",
                    TimeTags = new SortedDictionary<TextIndex, int?>
                    {
                        { new TextIndex(0), 1000 },
                        { new TextIndex(1), 2000 },
                        { new TextIndex(2), 3000 },
                        { new TextIndex(3), 4000 },
                        { new TextIndex(3, IndexState.End), 5000 },
                    },
                    RubyTags = new List<RubyTag>
                    {
                        new()
                        {
                            Text = "しま",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 1400}
                            },
                            StartIndex = 0,
                            EndIndex = 1
                        },
                        new()
                        {
                            Text = "しま",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                {new TextIndex(1), 2500}
                            },
                            StartIndex = 1,
                            EndIndex = 2
                        },
                        new()
                        {
                            Text = "しま",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                // will merge with second time-tag
                                {new TextIndex(1), 3500}
                            },
                            StartIndex = 2,
                            EndIndex = 3
                        },
                        new()
                        {
                            Text = "しま",
                            TimeTags = new SortedDictionary<TextIndex, int?>
                            {
                                // although the relative time is same as the first time-tag, but might not be able to merge.
                                {new TextIndex(1), 4400}
                            },
                            StartIndex = 3,
                            EndIndex = 4
                        }
                    }
                },
            }
        };

        var lrcText = new[]
        {
            "[00:01.00]島[00:02.00]島[00:03.00]島[00:04.00]島[00:05.00]",
            "",
            "@Ruby1=島,し[00:00.40]ま,,[00:02.00]",
            "@Ruby2=島,し[00:00.50]ま,[00:02.00],[00:03.00]",
            "@Ruby3=島,し[00:00.50]ま,[00:03.00],[00:04.00]",
            "@Ruby4=島,し[00:00.40]ま,[00:04.00]",
        };

        checkEncode(song, lrcText);
    }

    [Test]
    public void TestEncodeWithNoTimeRangeRuby()
    {
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

        var lrcText = new[]
        {
            "カラオケ",
            "",
            "@Ruby1=カ,か",
            "@Ruby2=ラ,ら",
            "@Ruby3=オ,お",
            "@Ruby4=ケ,け"
        };

        checkEncode(song, lrcText);
    }

    [Test]
    public void TestEncodeWithRubyInDifferentLine()
    {
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

        var lrcText = new[]
        {
            "[00:01.00]島[00:02.00]\n[00:03.00]島[00:04.00]\n[00:05.00]島[00:06.00]",
            "",
            "@Ruby1=島,しま,,[00:02.00]",
            "@Ruby2=島,じま,[00:03.00],[00:04.00]",
            "@Ruby3=島,とう,[00:05.00]"
        };

        checkEncode(song, lrcText);
    }

    [Test]
    public void TestEncodeWithEmptyFile()
    {
        var song = new Song();

        var lrcText = new[]
        {
            ""
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
        Assert.AreEqual(expected, actual);
    }
}
