// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using LrcParser.Model;
using LrcParser.Parser;
using LrcParser.Parser.Lines;
using NUnit.Framework;

namespace LrcParser.Tests.Parser;

public class LyricParserTest : BaseLyricParserTest<LyricParserTest.TestLyricParser>
{
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\n")]
    [TestCase(" \n ")]
    [TestCase(null)]
    public void TestDecodeWithEmptyFile(string lrcText)
    {
        var actual = Decode(lrcText);
        Assert.IsEmpty(actual.Lyrics);
    }

    public class TestLyricParser : LyricParser
    {
        public TestLyricParser()
        {
            Register<TestLineParser>();
        }

        protected override Song PostProcess(List<object> values)
        {
            var lines = values.OfType<string>();

            return new Song
            {
                Lyrics = lines.Select(l => new Lyric
                {
                    Text = l,
                }).ToList()
            };
        }

        protected override IEnumerable<object> PreProcess(Song song)
            => song.Lyrics.Select(lyric => lyric.Text);
    }

    private class TestLineParser : SingleLineParser<string>
    {
        public override bool CanDecode(string text)
            => true;

        public override string Decode(string text) => text;

        public override string Encode(string component, int index)
            => $"index:{index}, value: {component}";
    }
}
