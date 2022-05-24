// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace LrcParser.Model;

public class Lyric
{
    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; } = "";

    /// <summary>
    /// Time tags
    /// </summary>
    public SortedDictionary<TextIndex, int?> TimeTags { get; set; } = new();
}
