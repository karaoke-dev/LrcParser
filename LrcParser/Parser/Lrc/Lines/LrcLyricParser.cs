// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;
using LrcParser.Parser.Lines;

namespace LrcParser.Parser.Lrc.Lines;

public class LrcLyricParser : SingleLineParser<Lyric>
{
    public override Lyric Decode(string text)
    {
        throw new NotImplementedException();
    }

    public override string Encode(Lyric component)
    {
        throw new NotImplementedException();
    }
}
