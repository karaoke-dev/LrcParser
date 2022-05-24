// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;
using LrcParser.Parser;

namespace LrcParser.Tests.Parser;

public class BaseLyricParserTest<TParser> where TParser : LyricParser, new()
{
    protected Song Decode(string text) => new TParser().Decode(text);

    protected string Encode(Song component) => new TParser().Encode(component);
}
