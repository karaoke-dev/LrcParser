// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using LrcParser.Model;

namespace LrcParser.Parser;

/// <summary>
/// Base abstract class for encode/decode lyric format.
/// </summary>
public abstract class LyricParser
{
    /// <summary>
    /// Decode the lyric from the text.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public abstract Lyric Decode(string text);

    /// <summary>
    /// Encode the lyric to the text format.
    /// </summary>
    /// <param name="lyric"></param>
    /// <returns></returns>
    public abstract string Encode(Lyric lyric);
}
