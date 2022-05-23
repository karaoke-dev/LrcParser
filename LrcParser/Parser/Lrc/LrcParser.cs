// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;

namespace LrcParser.Parser.Lrc;

/// <summary>
/// Parser for encode and decode .lrc lyric format
/// </summary>
public class LrcParser : LyricParser
{
    public override Lyric Decode(string text)
    {
        throw new NotImplementedException();
    }

    public override string Encode(Lyric lyric)
    {
        throw new NotImplementedException();
    }
}
